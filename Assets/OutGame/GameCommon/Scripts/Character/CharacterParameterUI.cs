using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CharacterParameterUI
{
    [SerializeField] private ParameterUI _parameterUI;

    private int _currentParamTextSize = 50;
    private int _maxParamTextSize = 50;

    public void Init()
    {
        _parameterUI.ParamText.text = " ";

        var image = _parameterUI.ParamRankImage;
        var color = image.color;
        color.a = 0;
        image.color = color;

        SetParameterSlider(0, 0);
    }

    public void SetRankImage(Sprite rankSprite)
    {
        //アルファ値が1でなければ1にする
        if (_parameterUI.ParamRankImage.color.a != 1)
        {
            var color = _parameterUI.ParamRankImage.color;
            color.a = 1;
            _parameterUI.ParamRankImage.color = color;
        }

        _parameterUI.ParamRankImage.sprite = rankSprite;
    }

    public void SetParameterSlider(uint currentParam, uint nextRankValue)
    {
        uint currentRankMinValue = RankCalculator.GetCurrentRankMinNum(currentParam, CharacterParameterRankRateData.RankRateDict);

        _parameterUI.ParameterGage.maxValue = nextRankValue - currentRankMinValue;
        _parameterUI.ParameterGage.value = currentParam - currentRankMinValue;
    }

    public void SetParameterText(uint currentParam, uint nextRankValue)
    {
        _parameterUI.ParamText.text =
            $"<size={_currentParamTextSize}>{currentParam.ToString()}</size>\n" +
            $"<size={_currentParamTextSize}>{"/" + nextRankValue.ToString()}</size>";
    }

    [Serializable]
    public struct ParameterUI
    {
        public Slider ParameterGage;
        public Image ParamRankImage;
        public TMP_Text ParamText;
    }
}
