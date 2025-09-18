using System;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager _instance;
    public static InGameManager Instance => _instance;
    
    [SerializeField] private GameObject _characterIconPrefab;
    [SerializeField] private GameObject _characterBasePrefab;
    [SerializeField] private DebugDataManager _debugDataManager;
    public AIRoutes aiRoutes;                         //Enemyの出撃地点はIndex０、それ以降は敵が通るルート
    private bool _isPaused = false;                   //ポーズ中かどうか
    private float _timeSpeed = 1;                     //ゲーム内の時間の速さ
    public List<UnitBase> unitList;                   //ユニットのリスト
    private CharacterDeck _characterDeck;             //キャラクターの編成
    private Cell _selectedCell;                       //選択中のセル
    private GameObject _selectedCharacterObj;         //選択中のキャラクターObject
    private int _selectedCharacterID;                 //選択中のキャラクターID
    public CharacterDeck CharacterDeck => _characterDeck;
    private playerState _playerState = playerState.Idle;
    public event Action OnDropCharacter;
    public event Action OnSelectCharacter;

    #region UnityFunctions
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _characterDeck = new CharacterDeck(_debugDataManager.CharacterDatas);
        InstantiateCharacterIcons();
    }

    private void Update()
    {
        switch (_playerState)
        {
            case playerState.Idle:
            {
                break;
            }
            case playerState.DraggingCharacter:
            {
                DraggingCharacter();
                break;
            }
        }
        float timeSpeed = _timeSpeed * Time.deltaTime;
        UpdateUnits(timeSpeed);
        _characterDeck.UpdateTime(timeSpeed);
    }
    private void OnDestroy()
    {
        _instance = null;
    }
    #endregion
    
    #region UI Functions
    //アイコンを押下した時に呼び出される関数
    public void SelectCharacter(int characterID)
    {
        if (!_characterDeck.CanPlaceCharacter(characterID)) return;
        _selectedCharacterID = characterID;
        _selectedCharacterObj = Instantiate(_characterBasePrefab, transform);
        _selectedCharacterObj.transform.GetChild(0).GetComponent<Renderer>().material.color = _characterDeck.GetCharacterData(characterID).Color;
        _selectedCharacterObj.GetComponent<UnitBase>().ID = characterID;
        _playerState = playerState.DraggingCharacter;
        OnSelectCharacter?.Invoke();
    }

    //ドラッグ中の処理
    private void DraggingCharacter()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            _selectedCharacterObj.transform.position = hit.point;
            
            if(Input.GetButtonUp("Fire1"))
            {
                DropCharacter();
                return;
            }
            
            if (hit.transform.gameObject.CompareTag("Cell"))
            {
                Cell cell = hit.transform.GetComponent<Cell>();
                OnEnterCell(cell);
            }
            else
            {
                OnExitCell();
            }
        }
    }

    //ドラッグしたキャラクターをドロップ
    private void DropCharacter()
    {
        _playerState = playerState.Idle;
        OnDropCharacter?.Invoke();
        if (_selectedCell == null || !_selectedCell.CanPlaceCharacter())
        {
            Destroy(_selectedCharacterObj);
            return;
        };
        
        PlaceCharacter();
        OnExitCell();
    }

    private void PlaceCharacter()
    {
        _selectedCell.SetCharacter(_selectedCharacterObj);
        _selectedCharacterObj = null;
        _characterDeck.SetCanPlaceCharacter(_selectedCharacterID,false);
    }

    //マウスポインターがセルに入った時の処理
    private void OnEnterCell(Cell cell)
    {
        if(_selectedCell == cell) return;
        OnExitCell();
        _selectedCell = cell;
        _selectedCell.OnPointerEnter();
    }

    //マウスポインターがセルから出た時の処理
    private void OnExitCell()
    {
        if (_selectedCell == null) return;
        _selectedCell.OnPointerExit();
        _selectedCell = null;
    }
    
    //キャラクターアイコンを生成
    private void InstantiateCharacterIcons()
    {
        GameObject canvas = GameObject.Find("Canvas");
        float x = _characterIconPrefab.GetComponent<RectTransform>().rect.width;
        float y = _characterIconPrefab.GetComponent<RectTransform>().rect.height / 2;
        for (int i = 0; i < _characterDeck.Count; i++)
        {
            CharacterIcon characterIcon = Instantiate(_characterIconPrefab, new Vector3(x, y, 0), Quaternion.identity, canvas.transform).GetComponent<CharacterIcon>();
            x += _characterIconPrefab.GetComponent<RectTransform>().rect.width;
            characterIcon.SetID(i);
        }
    }
    #endregion

    #region Battle Functions
    private void UpdateUnits(float timeSpeed)
    {
        //ポーズ中ならば更新しない
        if (_isPaused) return;
        
        for(int i = 0; i < unitList.Count; i++)
        {
            UnitBase unit = unitList[i];
            if (unit.IsDead)
            {
                RemoveUnit(unit);
            }
            else
            {
                unit.UpdateUnit(timeSpeed);   
            }
        }
    }
    //ユニットを追加するメソッド
    public void AddUnit(UnitBase unit)
    {
        unitList.Add(unit);
    }
    //ユニットを削除するメソッド
    private void RemoveUnit(UnitBase unit)
    {
        unitList.Remove(unit);
        _characterDeck.CharacterRemoved(unit.ID);
        Destroy(unit.gameObject);
    }

    // 敵のユニットを出現するメソッド
    public void PlaceEnemyUnit(GameObject unitPrefab)
    {
        // 駒を生成する
        GameObject go = Instantiate(unitPrefab, aiRoutes.Points[0], Quaternion.identity);
        // プレイヤーの基地から出発
        go.transform.position = aiRoutes.Points[0];
        // ユニットの目標を設定する
        EnemyUnit unit = go.GetComponent<EnemyUnit>();
        unit.SetTargetPosition(aiRoutes.Points[1]);
        unit.Init();
    }
    
    //最寄りの敵対ユニットを返す
    public UnitBase FindNearestEnemy(UnitBase unit)
    {
        UnitBase nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (UnitBase enemy in unitList)
        {
            if (enemy.IsDead || !unit.IsEnemy(enemy))
            {   // 死んでいる敵は無視する
                continue;
            }

            float distance = unit.Distance(enemy);
            if (distance < nearestDistance)
            {   // 一番近い敵を覚えておく
                nearestEnemy = enemy;
                nearestDistance = distance;
            }
        }
        // 一番近い敵を返す
        return nearestEnemy;
    }
    //敵の次の目標地点を返す
    public Vector3 GetTargetRoutePosition(UnitBase unit, int index)
    {
        if (index >= aiRoutes.Points.Count)
        {
            RemoveUnit(unit);
            GetEnemyOnGoal();
            return new Vector3(0,0,0);
        }
        return aiRoutes.Points[index];
    }
    public void GetEnemyOnGoal()
    {
        Debug.Log("敵が防衛ラインを突破！！");
    }
    //時間の速さを変更する
    public void ChangeTimeSpeed(float timeSpeed)
    {
        _timeSpeed = timeSpeed;
    }
    #endregion
}

public enum playerState
{
    Idle,
    DraggingCharacter,
}
