using Cysharp.Threading.Tasks;
using DG.Tweening;
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

    [SerializeField] private float _uiPopSize;

    [SerializeField] private float _scaleUpTime;

    [SerializeField] private float _scaleDownTime;

    public ParameterUIData[] ParamUIData => _parameterUIData;

    public void SetParameter(ParameterType type, uint paramNum, RankData rankData)
    {
        ParameterUIData parameterUIData = FindParamUI(type);

        SetText(parameterUIData.CurrentParamText, paramNum);
        SetText(parameterUIData.MaxParamText, rankData.RankUpNum);
    }

    public ParameterUIData FindParamUI(ParameterType type)
    {
        foreach(var param in _parameterUIData)
        {
            if(param.ParamType == type)
            {
                return param;
            }
        }

        return null;
    }

    /// <summary> テキストの数値を変動させる </summary>
    public void SetText(TextMeshProUGUI currentParamText, uint currentParam)
    {
        string paramText = currentParam.ToString();
        currentParamText.text = paramText;
        UIPopAnimation(currentParamText.transform);
    }

    /// <summary> パラメータの数値を設定 </summary>
    public void MoveGage(Slider gageSlider, uint currentParam, float valueMoveSpeed)
    {
        gageSlider.DOValue(gageSlider.maxValue < currentParam ? gageSlider.maxValue : currentParam, valueMoveSpeed);
    }

    /// <summary> ゲージの最大値を設定 </summary>
    public void SetMaxGageValue(Slider paramGage, uint maxParam) => paramGage.maxValue = maxParam;

    /// <summary> Rankを設定 </summary>
    public void SetRank(Image rankImage, RankData rankdata)
    {
        rankImage.sprite = rankdata.RankSprite;
    }

    /// <summary> UIを一瞬だけ大きくして戻すアニメーション </summary>
    private void UIPopAnimation(Transform uiTransform)
    {
        var textScale = uiTransform.localScale;

        Sequence textSequence = DOTween.Sequence();

        textSequence.Append(uiTransform.DOScale(_uiPopSize, _scaleUpTime))
                    .Append(uiTransform.DOScale(textScale, _scaleDownTime))
                    .Play();
    }

    [System.Serializable]
    public class ParameterUIData
    {
        [SerializeField, Header("パラメータの種類")] private ParameterType _paramType;

        [SerializeField,Header("パラメータ表示テキスト")] 
        private TextMeshProUGUI _currentParamText;

        [SerializeField, Header("パラメータの最大値表示テキスト")]
        private TextMeshProUGUI _maxParamText;

        [SerializeField, Header("パラメータの強化予想数値表示テキスト")] 
        private TextMeshProUGUI _paramEnhanceText;

        [SerializeField, Header("パラメータのランクゲージ")] 
        private Slider _paramGage;

        [SerializeField, Header("パラメータのランクイメージ")] 
        private Image _rankImage;

        public ParameterType ParamType => _paramType;
        public TextMeshProUGUI CurrentParamText => _currentParamText;
        public TextMeshProUGUI MaxParamText => _maxParamText;
        public TextMeshProUGUI ParamEnhanceText => _paramEnhanceText;
        public Slider ParamGage => _paramGage;
        public Image RankImage => _rankImage;
    }
}
