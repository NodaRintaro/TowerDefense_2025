using System;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager _instance;
    public static InGameManager Instance => _instance;
    
    [SerializeField] private GameObject _characterIconPrefab;
    [SerializeField] private GameObject _characterBasePrefab;
    [SerializeField] private UnitDataManager unitDataManager;
    private Cell _selectedCell;
    private GameObject _selectedCharacterObj;
    private playerState _playerState = playerState.Idle;
    public UnitDataManager UnitDataManager => unitDataManager;
    public event Action OnDropCharacter;
    public event Action OnSelectCharacter;

    #region UnityMethod
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
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
    }
    #endregion
    #region UIMethod
    //アイコンを押下した時に呼び出される関数
    public void SelectCharacter(int characterID)
    {
        _selectedCharacterObj = Instantiate(_characterBasePrefab, transform);
        _selectedCharacterObj.transform.GetChild(0).GetComponent<Renderer>().material.color = unitDataManager.CharacterDatas[characterID].Color;
        _playerState = playerState.DraggingCharacter;
        OnSelectCharacter?.Invoke();
    }

    //ドラッグ中の処理
    void DraggingCharacter()
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
    void DropCharacter()
    {
        _playerState = playerState.Idle;
        OnDropCharacter?.Invoke();
        if (_selectedCell == null || !_selectedCell.SetCharacter(_selectedCharacterObj)) Destroy(_selectedCharacterObj);
        OnExitCell();
    }

    //マウスポインターがセルに入った時の処理
    void OnEnterCell(Cell cell)
    {
        if(_selectedCell == cell) return;
        OnExitCell();
        _selectedCell = cell;
        _selectedCell.OnPointerEnter();
    }

    //マウスポインターがセルから出た時の処理
    void OnExitCell()
    {
        if (_selectedCell == null) return;
        _selectedCell.OnPointerExit();
        _selectedCell = null;
    }
    
    //キャラクターアイコンを生成
    void InstantiateCharacterIcons()
    {
        GameObject canvas = GameObject.Find("Canvas");
        float x = _characterIconPrefab.GetComponent<RectTransform>().rect.width;
        float y = _characterIconPrefab.GetComponent<RectTransform>().rect.height / 2;
        for (int i = 0; i < unitDataManager.CharacterDatas.Length; i++)
        {
            CharacterIcon characterIcon = Instantiate(_characterIconPrefab, new Vector3(x, y, 0), Quaternion.identity, canvas.transform).GetComponent<CharacterIcon>();
            x += _characterIconPrefab.GetComponent<RectTransform>().rect.width;
            characterIcon.SetID(i);
        }
    }
    #endregion
}

public enum playerState
{
    Idle,
    DraggingCharacter,
}
