using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageView : MonoBehaviour
{
    [SerializeField, Header("ページを1つ戻るボタン")] private Button _backPageButton;
    [SerializeField, Header("ページを1つ進めるボタン")] private Button _nextPageButton;
    [SerializeField, Header("トレーニングを始めるボタン")] private Button _startTrainingButton;

    [SerializeField, Header("キャラクター選択ページ")]
    private GameObject _characterSelectPageObj;

    [SerializeField, Header("サポートカード選択ページ")]
    private GameObject _supportCardSelectPageObj;

    [SerializeField, Header("確認ページ")]
    private GameObject _selectedCharacterConfirmPageObj;

    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField] private Ease _pageFadeInEase = Ease.Linear;
    [SerializeField] private Ease _pageFadeOutEase = Ease.InBounce;

    private Dictionary<CharacterSelectPageType, GameObject> _characterSelectPageTypeDict = null;

    private const float _fadeOutPagePosX = 1500f;

    private CharacterSelectPageType _currentPageType = CharacterSelectPageType.None;

    public Button BackPageButton => _backPageButton;
    public Button NextPageButton => _nextPageButton;
    public Button StartTrainingButton => _startTrainingButton;

    private void OnEnable()
    {
        _characterSelectPageTypeDict = new();
        _characterSelectPageTypeDict.Add(CharacterSelectPageType.CharacterSelectPage, _characterSelectPageObj);
        _characterSelectPageTypeDict.Add(CharacterSelectPageType.SupportCardSelectPage, _supportCardSelectPageObj);
        _characterSelectPageTypeDict.Add(CharacterSelectPageType.SelectedCharacterConfirmPage, _selectedCharacterConfirmPageObj);

        //ボタンにアニメーションをつける   
        ButtonAnimation.SetupPointerEnterAnimationEvents(_nextPageButton);
        ButtonAnimation.SetupPointerEnterAnimationEvents(_backPageButton);
        ButtonAnimation.SetupPointerEnterAnimationEvents(_startTrainingButton);
    }

    private void OnDisable()
    {
        _characterSelectPageTypeDict = null;

        //ボタンのアニメーションを外す
        ButtonAnimation.RemoveAnimationEvent(_nextPageButton);
        ButtonAnimation.RemoveAnimationEvent(_backPageButton);
        ButtonAnimation.RemoveAnimationEvent(_startTrainingButton);
    }

    /// <summary> ほかのページへ移行する処理 </summary>
    public async UniTask TurnPage(CharacterSelectPageType pageType)
    {
        if (_currentPageType != CharacterSelectPageType.None)
        {
            await PageFadeOutAnimation(_characterSelectPageTypeDict[_currentPageType].transform as RectTransform);
            _characterSelectPageTypeDict[_currentPageType].SetActive(false);
        }

        ReleaseTurnPageButtonEvents();

        _characterSelectPageTypeDict[pageType].SetActive(true);
        _currentPageType = pageType;

        await PageFadeInAnimation(_characterSelectPageTypeDict[pageType].transform as RectTransform);
    }

    private void ReleaseTurnPageButtonEvents()
    {
        _backPageButton.onClick.RemoveAllListeners();
        _nextPageButton.onClick.RemoveAllListeners();
    }

    /// <summary> Pageの切り替え時のフェードアウトアニメーション </summary>
    private async UniTask PageFadeOutAnimation(RectTransform uiRectTransform)
    {
        Sequence seq = DOTween.Sequence();

        SetTurnPageButtonsInteractable(false, false);
        await seq.Join(uiRectTransform.DOAnchorPosX(_fadeOutPagePosX, _fadeDuration).SetEase(_pageFadeOutEase));
    }

    /// <summary> Pageの切り替え時のフェードインアニメーション </summary>
    private async UniTask PageFadeInAnimation(RectTransform uiRectTransform)
    {
        Vector2 startPos = new Vector2(_fadeOutPagePosX, uiRectTransform.anchoredPosition.y);
        Sequence seq = DOTween.Sequence();

        uiRectTransform.anchoredPosition = startPos;
        await seq.Join(uiRectTransform.DOAnchorPosX(0f, _fadeDuration).SetEase(_pageFadeInEase));
    }

    /// <summary> キャラクター選択画面のページ切り替えボタンのInteractableの状態を変更する関数 </summary>
    /// <param name="isNextPageButton">次のページへ進むボタンのInteractable</param>
    /// <param name="isBackPageButton">前ののページへ戻るボタンのInteractable</param>
    public void SetTurnPageButtonsInteractable(bool isNextPageButton, bool isBackPageButton)
    {
        _nextPageButton.interactable = isNextPageButton;
        _backPageButton.interactable = isBackPageButton;
    }

    /// <summary> トレーニング開始ボタンのInteractableを変更する関数 </summary>
    public void SetStartTrainingButtonInteractable(bool isInteractable)
    {
        _startTrainingButton.interactable = isInteractable;
    }
}
