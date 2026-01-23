using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingSelectScreen : ScreenBase
{
    [SerializeField, Header("フェードインする際のImage")]
    private Image _fadeImage;

    [SerializeField] private TrainingSelectButtonsPresenter _presenter;

    [SerializeField] private float _fadeDuration = 3f;

    public async override UniTask FadeInScreen()
    {
        if (!_fadeImage.gameObject.activeSelf)
            _fadeImage.gameObject.SetActive(true);

        Debug.Log("フェードイン");

        //フェードイン
        await _fadeImage.DOFade(0f, _fadeDuration).SetEase(Ease.InQuad).OnComplete(() => _fadeImage.gameObject.SetActive(false));
    }

    public async override UniTask FadeOutScreen()
    {
        _fadeImage.gameObject.SetActive(true);

        Debug.Log("フェードアウト");

        //フェードアウト
        await _fadeImage.DOFade(1f, _fadeDuration).SetEase(Ease.InSine);
    }
}
