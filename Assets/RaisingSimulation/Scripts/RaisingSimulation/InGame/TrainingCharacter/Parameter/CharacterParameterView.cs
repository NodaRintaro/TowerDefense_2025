using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// パラメータのUI管理クラス
/// </summary>
[System.Serializable]
public class CharacterParameterView
{
    [SerializeField, Header("各パラメータのUIデータ")]
    private ParameterUIData[] _parameterUIData;


    public void UpdateParamView()
    {

    }

    private ParameterUIData FindParamData(ParameterType type)
    {
        foreach(ParameterUIData paramUi in _parameterUIData)
        {
            if(paramUi.ParamType == type)
            {
                return paramUi;
            }
        }

        return null;
    }

    [System.Serializable]
    public class ParameterUIData
    {
        [SerializeField] private ParameterType _paramType;

        [SerializeField] private GameObject _parameterUiObj;

        [SerializeField] private TextMeshProUGUI _currentParamText;

        [SerializeField] private TextMeshProUGUI _maxParamText;

        [SerializeField] private TextMeshProUGUI _paramEnhanceNum;

        [SerializeField] private Image _currentParamGage;

        [SerializeField] private Image _rankImage;

        public ParameterType ParamType => _paramType;

        public void SetParameter(uint currentParam, uint maxParam)
        {
            _currentParamText.text = currentParam.ToString();
            _maxParamText.text = "/" + maxParam.ToString();
        }

        public void SetGage(uint currentParam, uint maxParam)
        {
            _currentParamGage.fillAmount = currentParam / maxParam;
        }

        public void SetRank(Sprite sprite)
        {
            _rankImage.sprite = sprite;
        }

        public void SetEnhanceNum(uint enhanceNum)
        {
            _paramEnhanceNum.text = "+" + enhanceNum.ToString();
        }
    }
}
