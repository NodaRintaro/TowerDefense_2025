using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public sealed class CharacterPickController : MonoBehaviour
{
    [SerializeField] CharacterBaseData _selectCharacterData;

    [SerializeField, Header("キャラクターの選択ボタンのサイズ")]
    private int _characterSelectButtonsize = 50;

    [SerializeField, Header("ステータスのスライダーのMaxValue")]
    private uint _maxSliderValue = 50;

    [SerializeField] private GameObject _characterSelectButtonObjPrefab;

    [SerializeField] private CharacterDataHolder _characterDataList;
    [SerializeField] private CharacterResource[] _characterResource;

    [SerializeField, Header("ViewClass")] 
    private CharacterPickUIHolder _characterPickUIHolder = new();

    private Dictionary<CharacterBaseData, CharacterResource> _characterInformationDict = new Dictionary<CharacterBaseData, CharacterResource>();
    private TrainingDataHolder _trainingDataHolder;
    private TrainingDataManager _trainingCharacterSaveDataManager;
    private TrainingDataSelectLifeTimeScope _characterPickLifeTimeScope;
    private SupportCardSelectController _supportCardSelectController;
    private CharacterSelectScreenChanger _screenChanger;

    private const string _characterDataPath = "CharacterResource/CharacterData/CharacterDataList";
    private const string _characterResoucesPath = "CharacterResource/CharacterSprite";

    public CharacterPickUIHolder CharacterPickUIHolder => _characterPickUIHolder;

    public Dictionary<CharacterBaseData, CharacterResource> CharacterInformationDict => _characterInformationDict;

    private async void Start()
    {
        _characterPickLifeTimeScope = FindAnyObjectByType<TrainingDataSelectLifeTimeScope>();
        _supportCardSelectController = _characterPickLifeTimeScope.Container.Resolve<SupportCardSelectController>();
        _screenChanger = _characterPickLifeTimeScope.Container.Resolve<CharacterSelectScreenChanger>();
        _trainingDataHolder = _characterPickLifeTimeScope.Container.Resolve<TrainingDataHolder>();

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
            if(character.CharacterID == id)
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

    private void RegisterTrainingCharacter(CharacterBaseData character)
    {
        _trainingDataHolder.SetCharacterData(character);
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
                if(data.CharacterID == resource.CharacterID && data.IsGetting == true)
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
                characterSelectButton = CharacterSelectButtonInstantiate(dictData.Key.CharacterID, iconSprite);
                characterSelectButton.onClick.AddListener(() => SelectTrainingCharacter(dictData.Key.CharacterID));
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

    private void ViewSelectCharacter(CharacterBaseData characterData)
    {
        foreach (var textData in _characterPickUIHolder.CharacterStatusTexts)
        {
            switch(textData.StatusType)
            {
                case ParameterType.Power:
                    textData.SetStatusText(characterData.BasePower.ToString());
                    _characterPickUIHolder.GetSliderData(ParameterType.Power).SetSlider(_maxSliderValue, characterData.BasePower);
                    break;
                case ParameterType.Physical:
                    textData.SetStatusText(characterData.BasePhysical.ToString());
                    _characterPickUIHolder.GetSliderData(ParameterType.Physical).SetSlider(_maxSliderValue, characterData.BasePhysical);
                    break;
                case ParameterType.Intelligence:
                    textData.SetStatusText(characterData.BaseIntelligence.ToString());
                    _characterPickUIHolder.GetSliderData(ParameterType.Intelligence).SetSlider(_maxSliderValue, characterData.BaseIntelligence);
                    break;
                case ParameterType.Speed:
                    textData.SetStatusText(characterData.BaseSpeed.ToString());
                    _characterPickUIHolder.GetSliderData(ParameterType.Speed).SetSlider(_maxSliderValue, characterData.BaseSpeed);
                    break;
                case ParameterType.ID:
                    textData.SetStatusText(characterData.CharacterID.ToString());
                    break;
                case ParameterType.Name:
                    textData.SetStatusText(characterData.CharacterName);
                    break;
                case ParameterType.RoleType:
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