using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager _instance;
    public static InGameManager Instance => _instance;

    [SerializeField] private GameObject _characterIconPrefab;
    [SerializeField] private GameObject _characterBasePrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _enemyTowerPrefab;          //敵の基地
    [SerializeField] private GameObject _playerTowerPrefab;         //プレイヤーの基地
    [SerializeField] private TextMeshProUGUI _towerHealthText;      //タワーの耐久値のText
    [SerializeField] private TextMeshProUGUI _timeSpeedText;        //タワーの名前のText
    [SerializeField] private TextMeshProUGUI _remainingEnemyText;   //残りの敵の数のText
    [SerializeField] private TextMeshProUGUI _coinText;              //コインのText
    [SerializeField] private DebugDataManager _debugDataManager;    //デバッグ用のデータ格納庫
    [SerializeField] public StageData stageData;                    //ステージのデータ
    private List<UnitBase> _unitList = new List<UnitBase>();        //ユニットのリスト
    private UnitDeck _unitDeck;                                     //キャラクターの編成
    private Cell _selectedCell;                                     //選択中のセル
    private GameObject _selectedCharacterObj;                       //選択中のキャラクターObject
    private float _ingameTimer = 0;                                 //ゲーム内時間
    private float _timeSpeed = 0;                                   //ゲーム内の時間の速さ
    private int _selectedCharacterID;                               //選択中のキャラクターID
    private int[] _waveEnemyIndex;                                  //WaveData内の敵データのインデックス
    private playerState _playerState = playerState.Loading;         //プレイヤーの状態
    private int _towerHealth = 0;                                   //タワーの耐久値
    private int _remainingEnemyNums = 0;                            //残りの敵の数
    private int _maxEnemyNums = 0;                                  //敵の数
    private float _coinTimer = 0;
    private int coins = 0;
    private float _coinInterval = 1;

    #region Properties

    private float IngameTimer
    {
        get => _ingameTimer;
        set
        {
            _ingameTimer = value;
            OnTimeUpdated?.Invoke(value);
        }
    }

    private int TowerHealth
    {
        get => _towerHealth;
        set
        {
            _towerHealth = value;
            UpdateTowerHealthText(value);
        }
    }

    private int RemainingEnemyNums
    {
        get => _remainingEnemyNums;
        set
        {
            _remainingEnemyNums = value;
            UpdateRemainingEnemyText(value);
        }
    }

    #endregion

    #region Events

    public event Action OnDropCharacter;
    public event Action OnSelectCharacter;

    /// <summary>
    /// ゲーム開始からの経過時間が更新された時に呼び出される
    /// </summary>
    public event Action<float> OnTimeUpdated;

    /// <summary>
    /// 前フレームからの経過時間が更新された時に呼び出される
    /// </summary>
    public event Action<float> OnPreviousTimeUpdated;

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
        // Instantiate(_enemyTowerPrefab, aiRoute.Points[0], Quaternion.identity);
        // Instantiate(_playerTowerPrefab, aiRoute.Points[aiRoute.Count-1], Quaternion.identity);
        _ = StageInitialize();

        //イベント関数への登録
        OnPreviousTimeUpdated += UpdateUnits;
        OnPreviousTimeUpdated += _unitDeck.UpdateTime;
        OnTimeUpdated += GenerateEnemyUnit;
        OnPreviousTimeUpdated += UpdateCoins;
    }

    private void Update()
    {
        switch (_playerState)
        {
            case playerState.Loading:
                {
                    break;
                }
            case playerState.Idle:
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.CompareTag("PlayerUnit"))
                            {
                                RemovePlayerUnit(hit.transform.gameObject.GetComponent<UnitBase>());
                            }
                        }
                    }

                    if (Input.GetButton("Cancel"))
                    {
                        _timeSpeed = 0;
                        _playerState = playerState.Paused;
                    }
                    break;
                }
            case playerState.DraggingCharacter:
                {
                    DraggingCharacter();
                    break;
                }
            case playerState.Paused:
                {
                    if (Input.GetButton("Cancel"))
                    {
                        _timeSpeed = 1;
                        _playerState = playerState.Idle;
                    }
                    break;
                }
            case playerState.GameOver:
                {
                    break;
                }
        }

        float ingameDeltaTime = _timeSpeed * Time.deltaTime;
        IngameTimer += ingameDeltaTime;
        OnPreviousTimeUpdated?.Invoke(ingameDeltaTime);
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

            if (Input.GetButtonUp("Fire1"))
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
        }
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
        _unitDeck.SetCanPlaceCharacter(_selectedCharacterID, false);
    }

    //マウスポインターがセルに入った時の処理
    private void OnEnterCell(Cell cell)
    {
        if (_selectedCell == cell) return;
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
            CharacterIcon characterIcon =
                Instantiate(_characterIconPrefab, new Vector3(x, y, 0), Quaternion.identity, canvas.transform)
                    .GetComponent<CharacterIcon>();
            x += _characterIconPrefab.GetComponent<RectTransform>().rect.width;
            characterIcon.SetID(i);
        }
    }

    private void UpdateTowerHealthText(int health)
    {
        _towerHealthText.text = $"Tower Health:{health.ToString()}";
    }

    private void UpdateRemainingEnemyText(int remainingEnemyNums)
    {
        _remainingEnemyText.text = $"Remaining Enemy:{remainingEnemyNums}/{_maxEnemyNums}";
    }

    #endregion

    #region Battle Functions

    private void UpdateUnits(float timeSpeed)
    {
        //ポーズ中ならば更新しない
        if (_playerState == playerState.Paused) return;

        for (int i = 0; i < _unitList.Count; i++)
        {
            UnitBase unit = _unitList[i];
            if (unit.IsDead)
            {
                RemovePlayerUnit(unit);
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
    private void RemovePlayerUnit(UnitBase unit)
    {
        unit.Remove();
        _unitList.Remove(unit);
        _unitDeck.CharacterRemoved(unit.PlayerData.ID);
        Destroy(unit.gameObject);
    }

    private void RemoveEnemyUnit(EnemyUnit unit)
    {
        unit.Remove();
        _unitList.Remove(unit);
        Destroy(unit.gameObject);
    }

    //敵のユニットを出現するメソッド
    public void GenerateEnemyUnit(float time)
    {
        for (int i = 0; i < stageData.waveDatas.Length; i++)
        {
            WaveData waveData = stageData.waveDatas[i];
            while (waveData.IsOverGenerateTime(time, _waveEnemyIndex[i]))
            {
                // 駒を生成する
                GameObject enemyObj = Instantiate(_enemyPrefab, waveData.aiRoute.points[0], Quaternion.identity);
                // プレイヤーの基地から出発
                enemyObj.transform.position = waveData.aiRoute.points[0];
                // ユニットにデータを設定して初期化
                EnemyUnit unit = enemyObj.GetComponent<EnemyUnit>();
                unit.UnitData = waveData.GetEnemyUnitData(_waveEnemyIndex[i]);
                unit.SetRoute(ref waveData.aiRoute);
                unit.Init();
                _waveEnemyIndex[i]++;
                RemainingEnemyNums--;
                // 敵の数が0になったらWaveの終了
                if (waveData.IsWaveEnd(_waveEnemyIndex[i]))
                {
                    break;
                }
            }
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
            {
                // 死んでいる敵は無視する
                continue;
            }

            float distance = unit.Distance(enemy);
            if (distance < nearestDistance)
            {
                // 一番近い敵を覚えておく
                nearestEnemy = enemy;
                nearestDistance = distance;
            }
        }

        // 一番近い敵を返す
        return nearestEnemy;
    }
    //敵がタワーに到着したときに呼ばれる
    public void EnemyArriveGoal(EnemyUnit unit)
    {
        Debug.Log("敵がタワーに到着");
        RemoveEnemyUnit(unit);
        TowerDagame(1);
    }

    //時間の速さを変更する
    public void ChangeTimeSpeed(float timeSpeed)
    {
        _timeSpeed = timeSpeed;
        _timeSpeedText.text = $"TimeSpeed: {_timeSpeed}";
    }
    
    //タワーへのダメージ
    private void TowerDagame(int damage)
    {
        TowerHealth -= damage;
    }
    private async UniTask CreateStageObjects(StageData data)
    {
        GameObject parent = new GameObject("Stage");
        // ステージのオブジェクトを生成する
        // 敵の基地とプレイヤーの基地の生成
        for (int i = 0; i < data.width; i++)
        {
            for (int j = 0; j < data.height; j++)
            {
                CellData cellData = data.cellDatas[i + j * data.width];
                Vector3 position;
                if (cellData.cellType == CellType.Flat)
                {
                    position = new Vector3(i, 0, -j);
                }
                else if (cellData.cellType == CellType.High)
                {
                    position = new Vector3(i, 0.5f, -j);
                }
                else
                {
                    position = new Vector3(i, 0, -j);
                }

                Instantiate(_cellPrefab, position, Quaternion.identity, parent.transform);
            }
        }
    }

    #endregion

    private async UniTask StageInitialize()
    {
        _waveEnemyIndex = new int[stageData.waveDatas.Length];
        TowerHealth = stageData.towerHealth;
        _coinInterval = stageData.generateCoinSpeed;
        
        foreach (var variable in stageData.waveDatas)
        {
            RemainingEnemyNums += variable.EnemyNumsInWave;
            _maxEnemyNums += variable.EnemyNumsInWave;
        }
        
        await CreateStageObjects(stageData);
        _playerState = playerState.Idle;
        _timeSpeed = 1;
    }
    private void UpdateCoins(float deltaTime)
    {
        _coinTimer += deltaTime;
        if (_coinTimer >= _coinInterval)
        {
            _coinTimer -= _coinInterval;
            GenerateCoin(1);
        }
    }
    public void GenerateCoin(int count)
    {
        coins += count;
        _coinText.text = $"Coins: {coins}";
    }
}
public enum playerState
{
    Idle,
    DraggingCharacter,
    Paused,
    GameOver,
    Loading,
}