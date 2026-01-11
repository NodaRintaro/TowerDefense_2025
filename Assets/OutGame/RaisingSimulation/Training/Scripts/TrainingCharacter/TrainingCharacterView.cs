using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TrainingCharacterView : MonoBehaviour
{
    [Header("キャラクターのイメージ")]
    [SerializeField] private Image _characterImage;

    [Header("キャラクターの各種パラメータのUI")]
    [SerializeField, Header("筋力")] private CharacterParameterUI _powerParamUI;
    [SerializeField, Header("知力")] private CharacterParameterUI _intelligenceParamUI;
    [SerializeField, Header("体力")] private CharacterParameterUI _physicalParamUI;
    [SerializeField, Header("素早さ")] private CharacterParameterUI _speedParamUI;

    [Header("トレーニングによって増加するパラメータの予測数値を表示するTexts")]
    [SerializeField] private TMP_Text _powerTrainingAddParamNum;
    [SerializeField] private TMP_Text _intelligenceTrainingAddParamNum;
    [SerializeField] private TMP_Text _physicalTrainingAddParamNum;
    [SerializeField] private TMP_Text _speedTrainingAddParamNum;

    private RaisingSimulationLifeTimeScope _lifeTimeScope;

    //DataClass
    private AddressableCharacterJobImageDataRepository _addressableCharacterJobImageDataRepository;
    private AddressableRankImageDataRepository _addressableRankImageDataRepository;

    public void Awake()
    {
        _lifeTimeScope = FindFirstObjectByType<RaisingSimulationLifeTimeScope>();
        
        _addressableCharacterJobImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableCharacterJobImageDataRepository>();
        _addressableRankImageDataRepository = _lifeTimeScope.Container.Resolve<AddressableRankImageDataRepository>();
    }

    public void SetImage(Sprite characterSprite)
    {
        _characterImage.sprite = characterSprite;
    }

    public void SetTrainingCharacterParameter(TrainingCharacterData trainingCharacterData)
    {
        SetParameter(_powerParamUI, trainingCharacterData.TotalPower);
        SetParameter(_intelligenceParamUI, trainingCharacterData.TotalIntelligence);
        SetParameter(_physicalParamUI, trainingCharacterData.TotalPhysical);
        SetParameter(_speedParamUI, trainingCharacterData.TotalSpeed);
    }

    /// <summary> パラメータのUIに現在のパラメータを反映する処理 </summary>
    private void SetParameter(CharacterParameterUI parameterUI, uint currentParam)
    {
        RankType currentRank = RankCalculator.GetCurrentRank(currentParam, CharacterParameterRankRateData.RankRateDict);
        uint nextRankValue = RankCalculator.GetNextRankNum(currentParam, CharacterParameterRankRateData.RankRateDict);

        //現在のパラメータのRankを反映
        parameterUI.SetRankImage(_addressableRankImageDataRepository.GetSprite(currentRank));

        //スライダーゲージに現在のパラメータを反映
        parameterUI.SetParameterSlider(currentParam, nextRankValue);

        //テキストに現在のパラメータを入力
        parameterUI.SetParameterText(currentParam, nextRankValue);
    }



    /// <summary> Buffによって得られるパラメータ上昇値を表示する処理 </summary>
    public void SetParameterBuffText(int powerBuff, int intelligenceBuff, int physicalBuff, int speedBuff)
    {
        WriteBuffText(_powerTrainingAddParamNum, powerBuff);
        WriteBuffText(_intelligenceTrainingAddParamNum, intelligenceBuff);
        WriteBuffText(_physicalTrainingAddParamNum, physicalBuff);
        WriteBuffText(_speedTrainingAddParamNum, speedBuff);
    }

    private void WriteBuffText(TMP_Text text, int buff)
    {
        if (buff > 0)
        {
            text.text = ("+" + buff).ToString();
            TextAnimation.ScalePulse(text);
        }
        else
            text.text = " ";
    }
}
