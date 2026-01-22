using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラクター選択画面の段階選択をButtonのView
/// </summary>
public class SelectPageView : MonoBehaviour
{
    [SerializeField, Header("ページを1つ戻るボタン")] private Button _backPageButton;
    [SerializeField, Header("次へ進むボタン")] private Button _nextButton;

    [SerializeField, Header("キャラクター選択ページ")]
    private GameObject _characterSelectPageObj;

    [SerializeField, Header("サポートカード選択ページ")]
    private GameObject _supportCardSelectPageObj;

    [SerializeField, Header("確認ページ")]
    private GameObject _selectedCharacterConfirmPageObj;

    [SerializeField] private Sprite _trainingStartButtonSprite;
    [SerializeField] private Sprite _nextPageSprite;

    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField] private Ease _pageFadeInEase = Ease.Linear;
    [SerializeField] private Ease _pageFadeOutEase = Ease.InBounce;

    private Dictionary<CharacterSelectPageType, GameObject> _characterSelectPageTypeDict = null;

    private const float _fadeOutPagePosX = 1500f;

    private CharacterSelectPageType _currentPageType = CharacterSelectPageType.None;

    public Button BackPageButton => _backPageButton;
    public Button NextButton => _nextButton;

    private void OnEnable()
    {
        _characterSelectPageTypeDict = new();
        _characterSelectPageTypeDict.Add(CharacterSelectPageType.CharacterSelectPage, _characterSelectPageObj);
        _characterSelectPageTypeDict.Add(CharacterSelectPageType.SupportCardSelectPage, _supportCardSelectPageObj);
        _characterSelectPageTypeDict.Add(CharacterSelectPageType.SelectedCharacterConfirmPage, _selectedCharacterConfirmPageObj);

        SetTurnPageButtonsInteractable(false,false);

        //ボタンにアニメーションをつける   
        ButtonAnimation.SetupPointerEnterAnimationEvents(_nextButton);
        ButtonAnimation.SetupPointerEnterAnimationEvents(_backPageButton);
    }

    private void OnDisable()
    {
        _characterSelectPageTypeDict = null;

        //ボタンのアニメーションを外す
        ButtonAnimation.RemoveAnimationEvent(_nextButton);
        ButtonAnimation.RemoveAnimationEvent(_backPageButton);
    }

    /// <summary> ほかのページへ移行する処理 </summary>
    public async UniTask TurnPage(CharacterSelectPageType pageType)
    {
        //現在表示している選択ページをフェードアウトし非表示にする
        if (_currentPageType != CharacterSelectPageType.None)
        {
            await PageFadeOutAnimation(_characterSelectPageTypeDict[_currentPageType].transform as RectTransform);
            _characterSelectPageTypeDict[_currentPageType].SetActive(false);
        }

        //現在登録されているページ切り替えボタンのイベントを外す
        ReleaseTurnPageButtonEvents();

        //次のページのオブジェクトを表示する
        _characterSelectPageTypeDict[pageType].SetActive(true);
        _currentPageType = pageType;

        //表示しているページによってボタンのスプライトを変更する処理
        if (_currentPageType == CharacterSelectPageType.SelectedCharacterConfirmPage)
            _nextButton.image.sprite = _trainingStartButtonSprite;
        else
            _nextButton.image.sprite = _nextPageSprite;

        //次のページをフェードインする
        await PageFadeInAnimation(_characterSelectPageTypeDict[pageType].transform as RectTransform);
    }

    private void ReleaseTurnPageButtonEvents()
    {
        _backPageButton.onClick.RemoveAllListeners();
        _nextButton.onClick.RemoveAllListeners();
    }

    /// <summary> Pageの切り替え時のフェードアウトアニメーション </summary>
    private async UniTask PageFadeOutAnimation(RectTransform uiRectTransform)
    {
        Sequence seq = DOTween.Sequence();

        SetTurnPageButtonsInteractable(false, false);
        (this.transform as RectTransform).DOAnchorPosX(_fadeOutPagePosX, _fadeDuration).SetEase(_pageFadeOutEase);
        await seq.Join(uiRectTransform.DOAnchorPosX(_fadeOutPagePosX, _fadeDuration).SetEase(_pageFadeOutEase));
    }

    /// <summary> Pageの切り替え時のフェードインアニメーション </summary>
    private async UniTask PageFadeInAnimation(RectTransform uiRectTransform)
    {
        Vector2 startPos = new Vector2(_fadeOutPagePosX, uiRectTransform.anchoredPosition.y);
        Sequence seq = DOTween.Sequence();

        uiRectTransform.anchoredPosition = startPos;
        (this.transform as RectTransform).DOAnchorPosX(0f, _fadeDuration).SetEase(_pageFadeInEase);
        await seq.Join(uiRectTransform.DOAnchorPosX(0f, _fadeDuration).SetEase(_pageFadeInEase));
    }

    /// <summary> キャラクター選択画面のページ切り替えボタンのInteractableの状態を変更する関数 </summary>
    /// <param name="isNextPageButton">次のページへ進むボタンのInteractable</param>
    /// <param name="isBackPageButton">前ののページへ戻るボタンのInteractable</param>
    public void SetTurnPageButtonsInteractable(bool isNextPageButton, bool isBackPageButton)
    {
        _nextButton.interactable = isNextPageButton;
        _backPageButton.interactable = isBackPageButton;
    }
}
