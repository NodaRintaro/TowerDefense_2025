using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CharacterParameterUI
{
    [SerializeField] private ParameterUI[] _parameterUIHolder;

    [SerializeField] private int _currentParamTextSize = 150;
    [SerializeField] private int _maxParamTextSize = 100;

    public ParameterUI[] ParameterUIHolder => _parameterUIHolder;

    public void SetRankImage(ref Image paramRank, Sprite rankSprite)
    {
        paramRank.sprite = rankSprite;
    }

    public void SetParameterSlider(ref Slider slider, uint currentParam, uint nextRankValue)
    {
        uint currentRankMinValue = RankCalculator.GetCurrentRankMinNum(currentParam, CharacterParameterRankRateData.RankRateDict);

        slider.maxValue = nextRankValue - currentRankMinValue;
        slider.value = currentParam - currentRankMinValue;
    }

    public void SetParameterText(ref TMP_Text text, uint currentParam, uint nextRankValue)
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
        public TMP_Text ParamText;
    }
}
