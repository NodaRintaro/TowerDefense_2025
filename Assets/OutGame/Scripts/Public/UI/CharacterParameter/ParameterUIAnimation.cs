using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ParameterUIAnimation
{
    [SerializeField] private float _uiPopSize;

    [SerializeField] private float _scaleUpTime;

    [SerializeField] private float _scaleDownTime;

    /// <summary> テキストの数値を変動させる </summary>
    public void TextUpdate(TextMeshProUGUI currentParamText, uint currentParam)
    {
        string paramText = currentParam.ToString();
        currentParamText.text = paramText;
        TextPopAnimation(currentParamText.transform);
    }

    /// <summary> パラメータの数値を設定 </summary>
    public void MoveGage(Slider gageSlider, uint currentParam, float valueMoveSpeed)
    {
        gageSlider.DOValue(gageSlider.maxValue < currentParam ? gageSlider.maxValue : currentParam, valueMoveSpeed);
    }

    /// <summary> ゲージの最大値を設定 </summary>
    public void SetMaxGageNum(Slider paramGage, uint maxParam) => paramGage.maxValue = maxParam;

    /// <summary> Rankを設定 </summary>
    public void SetRank(Image rankImage, RankData rankdata)
    {
        rankImage.sprite = rankdata.RankSprite;
    }

    /// <summary> UIを一瞬だけ大きくして戻すアニメーション </summary>
    private void TextPopAnimation(Transform uiTransform)
    {
        var textScale = uiTransform.localScale;

        Sequence textSequence = DOTween.Sequence();

        textSequence.Append(uiTransform.DOScale(_uiPopSize, _scaleUpTime))
                    .Append(uiTransform.DOScale(textScale, _scaleDownTime))
                    .Play();
    }

}
