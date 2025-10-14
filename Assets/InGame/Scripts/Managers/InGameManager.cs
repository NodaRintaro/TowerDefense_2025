using System;
using System.Collections.Generic;
using UnityEngine;
public class InGameManager : MonoBehaviour
{
    private static InGameManager _instance;
    public static InGameManager Instance => _instance;
    
    [SerializeField] private GameObject _characterIconPrefab;
    [SerializeField] private GameObject _characterBasePrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyTowerPrefab;      //敵の基地
    [SerializeField] private GameObject _playerTowerPrefab;     //プレイヤーの基地
    [SerializeField] private DebugDataManager _debugDataManager;//デバッグ用のデータ格納庫
    //[SerializeField] public AIRoute aiRoute;                  //Enemyの出撃地点はIndex０、それ以降は敵が通るルート
    [SerializeField] public WaveData waveData;                  //敵の出現パターン
    private int _waveCount = 0;                                 //敵の出現回数(敵の出現パターンのIndex)
    private float _ingameTimer = 0;                             //ゲーム内時間
    private bool _isPaused = false;                             //ポーズ中かどうか
    private float _timeSpeed = 1;                               //ゲーム内の時間の速さ
    private List<UnitBase> _unitList = new List<UnitBase>();                 //ユニットのリスト
    private UnitDeck _unitDeck;                       //キャラクターの編成
    private Cell _selectedCell;                                 //選択中のセル
    private GameObject _selectedCharacterObj;                   //選択中のキャラクターObject
    private int _selectedCharacterID;                           //選択中のキャラクターID
    private playerState _playerState = playerState.Idle;        //プレイヤーの状態
    public UnitDeck UnitDeck => _unitDeck;

    private float IngameTimer
    {
        get => _ingameTimer;
        set
        {
            _ingameTimer = value;
            OnInGameTimeUpdated?.Invoke(value);
        }
    }
    #region Events
    public event Action OnDropCharacter;
    public event Action OnSelectCharacter;
    /// <summary>
    /// ゲーム開始からの経過時間が更新された時に呼び出される
    /// </summary>
    public event Action<float> OnInGameTimeUpdated;
    /// <summary>
    /// 前フレームからの経過時間が更新された時に呼び出される
    /// </summary>
    public event Action<float> OnIngameDeltaTimeUpdated;
    #endregion

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
        _unitDeck = new UnitDeck(_debugDataManager.characterDatas);
        //アイコンの生成
        InstantiateCharacterIcons();
        //敵の基地とプレイヤーの基地の生成
        //Instantiate(_enemyTowerPrefab, aiRoute.Points[0], Quaternion.identity);
        //Instantiate(_playerTowerPrefab, aiRoute.Points[aiRoute.Count-1], Quaternion.identity);
        
        //イベント関数への登録
        OnIngameDeltaTimeUpdated += UpdateUnits;
        OnIngameDeltaTimeUpdated += _unitDeck.UpdateTime;
        OnInGameTimeUpdated += GenerateEnemyUnit;
    }

    private void Update()
    {
        switch (_playerState)
        {
            case playerState.Idle:
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    Debug.Log("Fire2");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log($"Hit: {hit.transform.gameObject.name}");
                        if (hit.transform.gameObject.CompareTag("PlayerUnit"))
                        {
                            RemoveUnit(hit.transform.gameObject.GetComponent<UnitBase>());
                        }
                    }
                }
                break;
            }
            case playerState.DraggingCharacter:
            {
                DraggingCharacter();
                break;
            }
        }
        
        float ingameDeltaTime = _timeSpeed * Time.deltaTime;
        IngameTimer += ingameDeltaTime;
        OnIngameDeltaTimeUpdated?.Invoke(ingameDeltaTime);
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
        if (!_unitDeck.CanPlaceCharacter(characterID)) return;
        _selectedCharacterID = characterID;
        _selectedCharacterObj = Instantiate(_characterBasePrefab, transform);
        //_selectedCharacterObj.transform.GetChild(0).GetComponent<Renderer>().material.color = _characterDeck.GetCharacterData(characterID).color;
        //_selectedCharacterObj.GetComponent<UnitBase>() = characterID;
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
        UnitBase unit = _selectedCharacterObj.GetComponent<PlayerUnit>();
        unit.UnitData = _unitDeck.GetCharacterData(_selectedCharacterID);
        unit.Init();
        
        _selectedCell.SetCharacter(_selectedCharacterObj);
        _selectedCharacterObj = null;
        _unitDeck.SetCanPlaceCharacter(_selectedCharacterID,false);
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
        for (int i = 0; i < _unitDeck.Count; i++)
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
        
        for(int i = 0; i < _unitList.Count; i++)
        {
            UnitBase unit = _unitList[i];
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
        _unitList.Add(unit);
    }
    //ユニットを削除するメソッド
    private void RemoveUnit(UnitBase unit)
    {
        unit.Remove();
        _unitList.Remove(unit);
        _unitDeck.CharacterRemoved(unit.UnitData.ID);
        Destroy(unit.gameObject);
    }

    // 敵のユニットを出現するメソッド
    public void GenerateEnemyUnit(float time)
    {
        // 敵の出現パターンのIndexを更新する  
        if (waveData.IsOverGenerateTime(_ingameTimer, _waveCount))
        {
            // 駒を生成する
            //GameObject enemyObj = Instantiate(_enemyPrefab, aiRoute.Points[0], Quaternion.identity);
            // プレイヤーの基地から出発
            //enemyObj.transform.position = aiRoute.Points[0];
            // ユニットの目標を設定する
            //EnemyUnit unit = enemyObj.GetComponent<EnemyUnit>();
            //unit.SetTargetPosition(aiRoute.Points[1]);
            // ユニットのデータを設定する
            //unit.UnitData = new EnemyUnitData(_debugDataManager.enemyDatas[0]);
            //unit.Init();
            
            // 敵の出現パターンのIndexを更新する
            _waveCount++;
            if (_waveCount >= waveData.Count)
            {
                // 出現パターンのIndexが最大値を超えたら終了
                OnInGameTimeUpdated -= GenerateEnemyUnit;
                return;
            }
            //生成し終えたらカウントを増やし出撃するタイミングが同じ場合も考慮し、もう一度GenerateEnemyUnit関数を呼ぶ
            GenerateEnemyUnit(time);
        }
    }
    
    //最寄りの敵対ユニットを返す
    public UnitBase FindNearestEnemy(UnitBase unit)
    {
        UnitBase nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (UnitBase enemy in _unitList)
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
    // public Vector3 GetTargetRoutePosition(UnitBase unit, int index)
    // {
    //     if (index >= aiRoute.Count)
    //     {
    //         RemoveUnit(unit);
    //         GetEnemyOnGoal();
    //         return new Vector3(0,0,0);
    //     }
    //     return aiRoute.Points[index];
    // }
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
