using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class TrainingEventFadeView : MonoBehaviour
{
    [SerializeField, Header("フェードインする際のImage")]
    private Image _fadeImage;

    [SerializeField] private float _fadeDuration = 2f;

    public async UniTask FadeIn()
    {
        if (!_fadeImage.gameObject.activeSelf)
            _fadeImage.gameObject.SetActive(true);

        //フェードイン
        _fadeImage.DOFade(0f, _fadeDuration).SetEase(Ease.InQuad).OnComplete(() => _fadeImage.gameObject.SetActive(false));
        await UniTask.WaitUntil(() => _fadeImage.gameObject.activeSelf);
    }

    public async UniTask FadeOut()
    {
        bool isCompleteFadeOut = false;
        _fadeImage.gameObject.SetActive(true);

        //フェードアウト
        _fadeImage.DOFade(1f, _fadeDuration).SetEase(Ease.InSine).OnComplete(() => isCompleteFadeOut = true);
        await UniTask.WaitUntil(() => isCompleteFadeOut);
    }
}
