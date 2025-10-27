using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public sealed class CharacterPickController : MonoBehaviour
{
    [SerializeField] CharacterData _selectCharacterData;

    [SerializeField] private string _characterDataPath;
    [SerializeField] private string _characterResoucesPath;

    [SerializeField, Header("キャラクターの選択ボタンのサイズ")]
    private int _characterSelectButtonsize = 50;

    [SerializeField, Header("ステータスのスライダーのMaxValue")]
    private uint _maxSliderValue = 50;

    [SerializeField] private GameObject _characterSelectButtonObjPrefab;

    [SerializeField] private CharacterDataHolder _characterDataList;
    [SerializeField] private CharacterResource[] _characterResource;

    [SerializeField, Header("ViewClass")] 
    private CharacterPickUIHolder _characterPickUIHolder = new();

    private Dictionary<CharacterData, CharacterResource> _characterInformationDict = new Dictionary<CharacterData, CharacterResource>();

    private TrainingDataSelectLifeTimeScope _characterPickLifeTimeScope;
    private SupportCardSelectController _supportCardSelectController;
    private ScreenChanger _screenChanger;

    public string CharacterDataPath => _characterDataPath;

    public CharacterPickUIHolder CharacterPickUIHolder => _characterPickUIHolder;

    public Dictionary<CharacterData, CharacterResource> CharacterInformationDict => _characterInformationDict;

    private async void Start()
    {
        _characterPickLifeTimeScope = FindAnyObjectByType<TrainingDataSelectLifeTimeScope>();
        _supportCardSelectController = _characterPickLifeTimeScope.Container.Resolve<SupportCardSelectController>();
        _screenChanger = _characterPickLifeTimeScope.Container.Resolve<ScreenChanger>();

        //キャラクターの情報を取得
        _characterDataList = await CharacterDataLoad(_characterDataPath);
        _characterResource = await CharacterResourcesLoad(_characterResoucesPath);

        await UniTask.WaitUntil(() => _characterPickLifeTimeScope.Container != null);

        InitUIEventActions();
    }

    /// <summary>
    /// Eventの初期化処理
    /// </summary>
    public void InitUIEventActions()
    {
        InitCharacterInformation();

        //CharacterSelectButtonを生成
        _characterPickUIHolder.ChangeButtonCellSize(_characterSelectButtonsize);
        CreateCharacterSelectButtons();

        //ButtonのにEventを追加
        SetCharacterPickAction();
    }

    private async UniTask<CharacterDataHolder> CharacterDataLoad(string path)
    {
        var resource = Resources.LoadAsync<CharacterDataHolder>(path);
        await resource;
        return resource.asset as CharacterDataHolder;
    }

    private async UniTask<CharacterResource[]> CharacterResourcesLoad(string path)
    {
        // パス配下のすべてのアセットを取得
        Object[] loadResource = Resources.LoadAll(path, typeof(CharacterResource));
        CharacterResource[] result = new CharacterResource[loadResource.Length];

        for (int i = 0; i < loadResource.Length; i++)
        {
            // ちょっとずつ非同期的に処理する（実際の読み込みは同期だが負荷を分散）
            await UniTask.Yield();

            result[i] = loadResource[i] as CharacterResource;
        }

        return result;
    }


    public void SelectTrainingCharacter(uint id)
    {
        foreach(var character in _characterDataList.DataList)
        {
            if(character.ID == id)
            {
                _selectCharacterData = character;
                Sprite characterSprite = _characterInformationDict[character].GetCharacterSprite(SpriteType.OverAllView);
                ViewSelectCharacter(character);
                _characterPickUIHolder.SetCharacterSprite(characterSprite);
                break;
            }
        }
    }

    private void SetCharacterPickAction()
    {
        _characterPickUIHolder.CharacterPickButton.onClick.AddListener(PickTrainingCharacter);
    }

    public void PickTrainingCharacter()
    {
        if (_selectCharacterData != null)
        {
            RegisterTrainingCharacter(_selectCharacterData);
            _screenChanger.ShowSupportCardPickScreen();
        }
        else Debug.Log("キャラクターが選ばれていません");
    }

    private void RegisterTrainingCharacter(CharacterData character)
    {
        TrainingDataHolder.SetCharacterData(character);
    }

    private void InitCharacterInformation()
    {
        if(_characterInformationDict != null)
        {
            _characterInformationDict = new();
        }

        foreach (var data in _characterDataList.DataList)
        {
            foreach(var resource in _characterResource)
            {
                if(data.ID == resource.CharacterID && data.IsGetting == true)
                {
                    _characterInformationDict.Add(data, resource);
                }
            }
        }
    }

    private void CreateCharacterSelectButtons()
    {
        foreach (var dictData in _characterInformationDict)
        {
            Button characterSelectButton;
            Sprite iconSprite = dictData.Value.GetCharacterSprite(SpriteType.Icon);

            if (iconSprite != null)
            {
                characterSelectButton = CharacterSelectButtonInstantiate(dictData.Key.ID, iconSprite);
                characterSelectButton.onClick.AddListener(() => SelectTrainingCharacter(dictData.Key.ID));
            }
        }
    }

    private Button CharacterSelectButtonInstantiate(uint id, Sprite sprite)
    {
        GameObject buttonObj = Instantiate(_characterSelectButtonObjPrefab);
        Image buttonImage = buttonObj.GetComponent<Image>();

        buttonImage.sprite = sprite;
        _characterPickUIHolder.AddCharacterSelectButton(buttonObj);
        
        return buttonObj.GetComponent<Button>();
    }

    private void ViewSelectCharacter(CharacterData characterData)
    {
        foreach (var textData in _characterPickUIHolder.CharacterStatusTexts)
        {
            switch(textData.StatusType)
            {
                case StatusType.Power:
                    textData.SetStatusText(characterData.Power.ToString());
                    _characterPickUIHolder.GetSliderData(StatusType.Power).SetSlider(_maxSliderValue, characterData.Power);
                    break;
                case StatusType.Physical:
                    textData.SetStatusText(characterData.Physical.ToString());
                    _characterPickUIHolder.GetSliderData(StatusType.Physical).SetSlider(_maxSliderValue, characterData.Physical);
                    break;
                case StatusType.Intelligence:
                    textData.SetStatusText(characterData.Intelligence.ToString());
                    _characterPickUIHolder.GetSliderData(StatusType.Intelligence).SetSlider(_maxSliderValue, characterData.Intelligence);
                    break;
                case StatusType.Speed:
                    textData.SetStatusText(characterData.Speed.ToString());
                    _characterPickUIHolder.GetSliderData(StatusType.Speed).SetSlider(_maxSliderValue, characterData.Speed);
                    break;
                case StatusType.ID:
                    textData.SetStatusText(characterData.ID.ToString());
                    break;
                case StatusType.Name:
                    textData.SetStatusText(characterData.CharacterName);
                    break;
                case StatusType.RoleType:
                    textData.SetStatusText(characterData.RoleType.ToString());
                    break;
            }
        }
    }
}

public enum CharacterPickScreen
{
    CharacterSelect,
    SupportFormationSelect,
    SupportSelect
}