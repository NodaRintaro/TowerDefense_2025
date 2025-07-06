using System;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager _instance;
    public static InGameManager Instance => _instance;
    
    [SerializeField] private GameObject _characterButtonPrefab;
    [SerializeField] private CharacterDataManager _characterDataManager;
    private int _selectedCharacterID = -1;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        InstantiateCharacterButtons();
    }

    void InstantiateCharacterButtons()
    {
        GameObject canvas = GameObject.Find("Canvas");
        float x = _characterButtonPrefab.GetComponent<RectTransform>().rect.width;
        float y = _characterButtonPrefab.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < _characterDataManager.CharacterDatas.Length; i++)
        {
            CharacterButton characterButton = Instantiate(_characterButtonPrefab, new Vector3(x, y, 0), Quaternion.identity, canvas.transform).GetComponent<CharacterButton>();
            x += _characterButtonPrefab.GetComponent<RectTransform>().rect.width;
            characterButton.id = i;
        }
    }

    public void SelectCharacter(int characterID)
    {
        _selectedCharacterID = characterID;
        Debug.Log("Selected Character ID: " + _selectedCharacterID);
    }
}
