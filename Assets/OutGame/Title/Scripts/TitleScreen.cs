using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _fadeImage;

    [SerializeField] private float _fadeDuration;

    private bool _isSceneChange = false;
    private const string _homeSceneName = "Home";

    public async void OnPointerClick(PointerEventData pointerEventData)
    {
        if (_isSceneChange)
        {
            await FadeOutScreen();
            SceneChanger.SceneChange(_homeSceneName);
        }
    }

    public async UniTask FadeOutScreen()
    {
        bool isCompleteFadeOut = false;
        _fadeImage.gameObject.SetActive(true);

        //フェードアウト
        _fadeImage.DOFade(1f, _fadeDuration).SetEase(Ease.InSine).OnComplete(() => isCompleteFadeOut = true);
        await UniTask.WaitUntil(() => isCompleteFadeOut);
    }

    public void IsSceneChange(bool isSceneChange) { _isSceneChange = isSceneChange; }
}
