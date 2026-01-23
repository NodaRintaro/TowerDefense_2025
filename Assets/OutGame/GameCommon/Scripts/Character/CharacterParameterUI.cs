using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CharacterParameterUI
{
    [SerializeField] private ParameterUI _parameterUI;

    [SerializeField] private int _currentParamTextSize = 50;

    private uint _currentParamValue; // 現在の値を保持する変数

    private const float _duration = 0.5f;

    public void Init()
    {
        _parameterUI.ParamText.text = " ";
        _parameterUI.ParameterGage.value = 0;
        _currentParamValue = 0;

        var image = _parameterUI.ParamRankImage;
        var color = image.color;
        color.a = 0;
        image.color = color;
    }

    /// <summary>
    /// パラメータを設定し、UIをアニメーションさせる
    /// </summary>
    public async UniTask SetParameter(uint newParam, uint currentRankMinValue, uint nextRankValue, Sprite rankSprite)
    {
        await UpdateRankImage(rankSprite);

        // スライダーのアニメーション
        _parameterUI.ParameterGage.maxValue = nextRankValue - currentRankMinValue;
        var sliderCompletionSource = new UniTaskCompletionSource();
        await _parameterUI.ParameterGage.DOValue(newParam - currentRankMinValue, _duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => sliderCompletionSource.TrySetResult());
        var sliderTask = sliderCompletionSource.Task;

        // テキストのカウントアップアニメーション
        var textAnimationCompletionSource = new UniTaskCompletionSource();
        await DOTween.To(() => _currentParamValue, x => _currentParamValue = x, newParam, _duration)
            .OnUpdate(() =>
            {
                _parameterUI.ParamText.text =
                    $"<size={_currentParamTextSize}>{_currentParamValue.ToString()}</size>\n" +
                    $"<size={_currentParamTextSize}>{"/" + nextRankValue.ToString()}</size>";
            })
            .OnComplete(() => textAnimationCompletionSource.TrySetResult());
        var textAnimationTask = textAnimationCompletionSource.Task;

        // テキストの強調アニメーション
        var textScaleCompletionSource = new UniTaskCompletionSource();
        await _parameterUI.ParamText.transform.DOScale(1.2f, 0.1f)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => textScaleCompletionSource.TrySetResult());
        var textScaleTask = textScaleCompletionSource.Task;

        // すべてのアニメーションタスクを待機
        await UniTask.WhenAll(sliderTask, textAnimationTask, textScaleTask);

        _currentParamValue = newParam; // 最終的な値を設定
    }

    private async UniTask UpdateRankImage(Sprite newRankSprite)
    {
        if(_parameterUI.ParamRankImage.color.a == 0)
        {
            Color color = _parameterUI.ParamRankImage.color;
            color.a = 255;
            _parameterUI.ParamRankImage.color = color;
        }

        if(_parameterUI.ParamRankImage.sprite != newRankSprite)
        {
            Vector3 originalPosition = _parameterUI.ParamRankImage.transform.localPosition;

            // Sequenceを構築してから一度に再生します
            Sequence seq = DOTween.Sequence();
            seq.Append(_parameterUI.ParamRankImage.transform.DOLocalMove(new Vector3(-50, 50, 0), 0.3f).SetRelative())
                .Join(_parameterUI.ParamRankImage.transform.DOLocalRotate(new Vector3(0, 0, 15), 0.3f))
                .Append(_parameterUI.ParamRankImage.transform.DOScale(0, 0.2f))
                .AppendCallback(() =>
                {
                    _parameterUI.ParamRankImage.sprite = newRankSprite;
                    _parameterUI.ParamRankImage.transform.localPosition = originalPosition + new Vector3(0, 20, 0);
                })
                .Append(_parameterUI.ParamRankImage.transform.DOScale(1.0f, 0.3f))
                .Join(_parameterUI.ParamRankImage.transform.DOLocalMove(originalPosition, 0.3f).SetEase(Ease.OutBounce))
                .Join(_parameterUI.ParamRankImage.transform.DOLocalRotate(Vector3.zero, 0.3f));

            // 構築したSequenceを再生し、完了を待機します
            await seq.Play().ToUniTask();
        }
    }

    [Serializable]
    public struct ParameterUI
    {
        public Slider ParameterGage;
        public Image ParamRankImage;
        public TMP_Text ParamText;
    }
}