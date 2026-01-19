using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CharacterSelectScreen : ScreenBase
{
    [SerializeField] private PageView _pageView;
    [SerializeField] private GameObject _selectedCharacterObj;

    [SerializeField, Header("フェードインする際のImage")] 
    private Image _fadeImage;

    [SerializeField] private float _fadeDuration = 3f;
    
    public async override UniTask FadeInScreen()
    {
        _fadeImage.gameObject.SetActive(true);
        _pageView.gameObject.SetActive(true);
        _selectedCharacterObj.SetActive(true);

        //フェードイン
        await _fadeImage.DOFade(0f, _fadeDuration).SetEase(Ease.InQuad).OnComplete(() => _fadeImage.gameObject.SetActive(false));
        await UniTask.WaitUntil(() => _fadeImage.gameObject.activeSelf);

        //フェードイン後に若干の間を設ける
        int fadeInDelay = 1000;
        await UniTask.Delay(fadeInDelay);

        await _pageView.TurnPage(CharacterSelectPageType.CharacterSelectPage);
    }

    public async override UniTask FadeOutScreen()
    {
        bool isCompleteFadeOut = false;
        _fadeImage.gameObject.SetActive(true);

        //フェードアウト
        await _fadeImage.DOFade(1f, _fadeDuration).SetEase(Ease.InSine).OnComplete(() => isCompleteFadeOut = true);
        await UniTask.WaitUntil(() => isCompleteFadeOut);

        _pageView.gameObject.SetActive(false);
        _selectedCharacterObj.SetActive(false);
    }
}
