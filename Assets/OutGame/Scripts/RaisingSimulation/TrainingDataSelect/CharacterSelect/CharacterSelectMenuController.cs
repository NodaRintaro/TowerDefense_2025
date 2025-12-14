using UnityEngine;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;
using CharacterSelectView;
using Cysharp.Threading.Tasks;
using CharacterData;

public class CharacterSelectMenuController : MonoBehaviour, IController
{
    [SerializeField,Header("ViewClass")] 
    private ViewData _view = default;

    //育成対象に選んだキャラクターのデータ
    private TrainingCharacterData _selectCharacterData = null;

    //育成ゲームのOutGame全般で使用するLifeTimeScope
    private RaisingSimulationOutGameLifeTimeScope _lifeTimeScope = null;

    //育成ゲームで使用するデータを生成するクラス
    private TrainingDataBuilder _trainingDataCreater = null;

    //キャラクター選択ボタンを生成するクラス
    private SelectButtonCreater _characterSelectButtonCreater = null;

    //画面切り替えクラス
    private ScreenChanger _screenChanger = null;

    #region 各種データClass
    //キャラクターの所持データ
    //private GettingDataRegistry _characterGettingData = null;

    //キャラクターのデータ
    private CharacterBaseDataRegistry _characterData = null;

    //キャラクターの画像データ
    private List<CharacterSpriteHolder> _characterSpriteDataList = null;
    #endregion

    public IViewData View => _view;

    [Inject]
    public CharacterSelectMenuController()
    {
        //VContainerの依存性注入用コンストラクタ
    }

    public void Start()
    {
        //lifetimeScope内のObjectを取得
        _lifeTimeScope = FindAnyObjectByType<RaisingSimulationOutGameLifeTimeScope>();
        ResolveContainerObj(_lifeTimeScope);
        

    }

    public void InitView()
    {

    }

    public void SetEvents()
    {

    }

    public async UniTask LoadAssets()
    {
        
    }

    /// <summary> LifeTimeScope内のClassをResolveする関数 </summary>
    private void ResolveContainerObj(LifetimeScope lifeTimeScope)
    {
        _trainingDataCreater = lifeTimeScope.Container.Resolve<TrainingDataBuilder>();
        _characterSelectButtonCreater = lifeTimeScope.Container.Resolve<SelectButtonCreater>();
        _screenChanger = lifeTimeScope.Container.Resolve<ScreenChanger>();
    }

    /// <summary> キャラクター選択ボタンを生成シーン上に生成する関数 </summary>
    private void CharacterSelectButtonInstantiate()
    {
        //_characterSelectButtonCreater.CreateButton()
    }

    /// <summary> キャラクター選択ボタンを押した際の処理を行う関数 </summary>
    private void OnCharacterSelectEvent(uint id)
    {
        
    }

    /// <summary> キャラクターのデータをViewに反映させる関数 </summary>
    private void SetCharacterInformation(CharacterBaseData characterBaseData, Sprite characterSprite)
    {
        CharacterInformationView charaInfo = _view.CharacterInformation;

        charaInfo.CharacterImage.sprite = characterSprite;
        charaInfo.NameText.text = characterBaseData.CharacterName;
        charaInfo.IdText.text = characterBaseData.CharacterID.ToString();
        charaInfo.RoleTypeText.text = characterBaseData.CharacterRole.ToString();

        foreach (var paramUI in charaInfo.ParameterUIArray)
        {
            SetParameterView(paramUI, characterBaseData);
        }
    }

    /// <summary> キャラクターのパラメータの数値をViewに反映させる関数 </summary>
    private void SetParameterView(CharacterInformationView.CharacterParameterUI parameterUI, CharacterBaseData characterBaseData)
    {
        switch (parameterUI.ParamType)
        {
            case ParameterType.Physical:
                
                break;
            case ParameterType.Power:
                
                break;
            case ParameterType.Intelligence:
                
                break;
            case ParameterType.Speed:
                
                break;
        }
    }
}