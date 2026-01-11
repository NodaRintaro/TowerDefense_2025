using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;

public class TextAnimation
{
    private static readonly HashSet<TMP_Text> _animatingTexts = new HashSet<TMP_Text>();
    public static void ScalePulse(TMP_Text text, CancellationToken cancellationToken = default)
    {
        if (_animatingTexts.Contains(text)) return;

        _animatingTexts.Add(text);

        Sequence sequence = null;

        float scaleFactor = 1.5f;
        float duration = 0.3f;
        Vector3 originalScale = text.transform.localScale;
        Vector3 targetScale = originalScale * scaleFactor;

        // DoTweenのSequenceを使用してアニメーションを組み立て
        sequence = DOTween.Sequence()
            .Append(text.transform.DOScale(targetScale, duration).SetEase(Ease.OutQuad))
            .Append(text.transform.DOScale(originalScale, duration).SetEase(Ease.InQuad))
            .OnComplete(() => _animatingTexts.Remove(text));
    }
}