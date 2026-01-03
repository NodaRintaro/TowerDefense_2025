using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : ScreenBase
{
    private float _fadeDuration = 1f;
    private CanvasGroup _fadeCanvas;

    public void Awake()
    {
        _fadeCanvas = GetComponent<CanvasGroup>();
    }

    public async override UniTask FadeOutScreen()
    {
        _fadeCanvas.gameObject.SetActive(true);

        _fadeCanvas.alpha = 1f;
        var dotween = _fadeCanvas.DOFade(0f, _fadeDuration)
            .SetEase(Ease.InOutQuad);

        await UniTask.WaitUntil(() => dotween.IsComplete());
    }

    public async override UniTask FadeInScreen()
    {
        _fadeCanvas.alpha = 0f;
        var dotween = _fadeCanvas.DOFade(1f, _fadeDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => _fadeCanvas.gameObject.SetActive(false));

        await UniTask.WaitUntil(() => dotween.IsComplete());
    }
}
