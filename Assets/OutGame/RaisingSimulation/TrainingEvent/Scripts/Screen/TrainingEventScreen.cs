using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class TrainingEventScreen : ScreenBase
{
    [SerializeField] private NovelEventPresenter _controller;

    [SerializeField, Header("フェードインする際のImage")]
    private Image _fadeImage;

    [SerializeField] private float _fadeDuration = 2f;

    public async override UniTask FadeInScreen()
    {
        if (!_fadeImage.gameObject.activeSelf)
            _fadeImage.gameObject.SetActive(true);

        //フェードイン
        await _fadeImage.DOFade(0f, _fadeDuration).SetEase(Ease.InQuad).OnComplete(() =>_fadeImage.gameObject.SetActive(false));    
    }

    public async override UniTask FadeOutScreen()
    {
        _fadeImage.gameObject.SetActive(true);

        //フェードアウト
        await _fadeImage.DOFade(1f, _fadeDuration).SetEase(Ease.InSine);
    }
}
