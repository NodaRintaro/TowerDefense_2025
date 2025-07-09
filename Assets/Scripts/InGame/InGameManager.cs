using System;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager _instance;
    public static InGameManager Instance => _instance;
    
    [SerializeField] private GameObject _characterIconPrefab;
    [SerializeField] private GameObject _characterBasePrefab;
    [SerializeField] private CharacterDataManager _characterDataManager;
    private int _selectedCharacterID = -1;
    private GameObject _selectedCharacterObj;
    private playerState _playerState = playerState.Idle;
    public CharacterDataManager CharacterDataManager => _characterDataManager;
    
    public event Action onDropCharacter;
    public event Action onSelectCharacter;

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
    
    //アイコンをポインターが押下した時に呼び出される関数
    public void SelectCharacter(int characterID)
    {
        _selectedCharacterID = characterID; 
        _selectedCharacterObj = Instantiate(_characterBasePrefab, transform);
        _playerState = playerState.DraggingCharacter;
        onSelectCharacter?.Invoke();
    }

    void DraggingCharacter()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            _selectedCharacterObj.transform.position = hit.point;
        }
        if(Input.GetButtonUp("Fire1"))
        {
            canDropCharacter();
        }
    }

    void canDropCharacter()
    {
        _playerState = playerState.Idle;
        onDropCharacter?.Invoke();
    }
    
    //キャラクターアイコンを生成
    void InstantiateCharacterIcons()
    {
        GameObject canvas = GameObject.Find("Canvas");
        float x = _characterIconPrefab.GetComponent<RectTransform>().rect.width;
        float y = _characterIconPrefab.GetComponent<RectTransform>().rect.height / 2;
        for (int i = 0; i < _characterDataManager.CharacterDatas.Length; i++)
        {
            CharacterIcon characterIcon = Instantiate(_characterIconPrefab, new Vector3(x, y, 0), Quaternion.identity, canvas.transform).GetComponent<CharacterIcon>();
            x += _characterIconPrefab.GetComponent<RectTransform>().rect.width;
            characterIcon.SetID(i);
        }
    }
}

public enum playerState
{
    Idle,
    DraggingCharacter,
}
