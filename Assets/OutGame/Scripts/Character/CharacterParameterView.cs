using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using CharacterData;

/// <summary>
/// キャラクターのパラメータの表示を行うViewClass
/// </summary>
public class CharacterParameterView : MonoBehaviour
{
    [SerializeField] ParameterUI[] _parameterUIHolder;

    [SerializeField] int _currentParamTextSize = 150;
    [SerializeField] int _maxParamTextSize = 100;

    public ParameterUI[] parameterUIHolder => _parameterUIHolder;

    public void Awake()
    {
        foreach (var parameter in _parameterUIHolder)
        {
            SetParameterUI(0, parameter);
        }
    }

    public void SetCharacterParameter(CharacterBaseData character)
    {
        foreach(var parameterUI in _parameterUIHolder)
        {
            switch(parameterUI.ParamType)
            {
                case ParameterType.Physical:
                    SetParameterUI(character.TotalPhysical, parameterUI);
                    break;
                case ParameterType.Power:
                    SetParameterUI(character.TotalPower, parameterUI);
                    break;
                case ParameterType.Intelligence:
                    SetParameterUI(character.TotalIntelligence, parameterUI);
                    break;
                case ParameterType.Speed:
                    SetParameterUI(character.TotalSpeed, parameterUI);
                    break;
            }
        }
    }

    public async void SetParameterUI(uint currentParam, ParameterUI parameterUI)
    {
        uint nextRankValue = RankCalculator.GetNextRankNum(currentParam, RankRateData.ParamRankRate);
        
        await SetRankImage(parameterUI.ParamRankImage, currentParam);
        SetParameterSlider(parameterUI.Slider, currentParam, nextRankValue);
        SetParameterText(parameterUI.ParamRankText, currentParam, nextRankValue);
    }

    private async UniTask SetRankImage(Image image, uint currentParam)
    {
        RankType currentRankType = RankCalculator.GetCurrentRank(currentParam, RankRateData.ParamRankRate);
        string rankSpriteAAGPath = AAGPathFinder.GetAAGPathWithEnum(currentRankType);
        image.sprite = await AssetsLoader.LoadAssetAsync<Sprite>(rankSpriteAAGPath);
    }

    private void SetParameterSlider(Slider slider, uint currentParam, uint nextRankValue)
    {
        uint currentRankMinValue = RankCalculator.GetCurrentRankMinNum(currentParam, RankRateData.ParamRankRate);

        slider.maxValue = nextRankValue - currentRankMinValue;
        slider.value = currentParam - currentRankMinValue;
    }

    private void SetParameterText(TMP_Text text, uint currentParam, uint nextRankValue)
    {
        text.text =
            $"<size={_currentParamTextSize}>{currentParam.ToString()}</size>\n /" +
            $"<size={_currentParamTextSize}>{nextRankValue.ToString()}</size>";
    }

    [Serializable]
    public struct ParameterUI
    {
        public ParameterType ParamType;
        public Slider Slider;
        public Image ParamRankImage;
        public TMP_Text ParamRankText;
    }
}
