using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : ScreenBase
{
    [SerializeField, Header("フェードインする際のImage")]
    private Image _fadeImage;

    [SerializeField] private float _fadeDuration = 3f;
    [SerializeField] private ScreenQuickFadeOut _screenQuickFadeOut;
    [SerializeField] private GameObject _homeMenuUI;
    [SerializeField] private GameObject _characterListUI;
    [SerializeField] private GameObject _teamBuildUI;
    [SerializeField] private GameObject _selectCharacterUI;
    
    private RectTransform _homeMenuRectTransform;
    private RectTransform _characterListRectTransform;
    private RectTransform _teamBuildUIRectTransform;
    private RectTransform _selectCharacterUIRectTransform;
    private RectTransform _currentActiveUI;

    public async override UniTask FadeInScreen()
    {
        if (!_fadeImage.gameObject.activeSelf)
            _fadeImage.gameObject.SetActive(true);

        //フェードイン
        _fadeImage.DOFade(1f, 0f);
        _fadeImage.DOFade(0f, _fadeDuration).SetEase(Ease.InQuad).OnComplete(() => _fadeImage.gameObject.SetActive(false));
        await UniTask.WaitUntil(() => _fadeImage.gameObject.activeSelf);
    }

    public async override UniTask FadeOutScreen()
    {
        bool isCompleteFadeOut = false;
        _fadeImage.gameObject.SetActive(true);

        //フェードアウト
        _fadeImage.DOFade(0f, 0f);
        _fadeImage.DOFade(1f, _fadeDuration).SetEase(Ease.InSine).OnComplete(() => isCompleteFadeOut = true);
        await UniTask.WaitUntil(() => isCompleteFadeOut);
    }

    public void FadeInScreenButton()
    {
        FadeInScreen();
    }
    
    public void FadeOutScreenButton()
    {
        FadeOutScreen();
    }
    
    public void ChangeToCharacterListScreen()
    {
        _screenQuickFadeOut.ChangeScreen(_currentActiveUI, _characterListRectTransform);
        _currentActiveUI = _characterListRectTransform;
    }
    
    public void ChangeToHomeMenuScreen()
    {
        _screenQuickFadeOut.ChangeScreen(_currentActiveUI, _homeMenuRectTransform);
        _currentActiveUI = _homeMenuRectTransform;
    }
    public void ChangeToTeamBuildScreen()
    {
        _screenQuickFadeOut.ChangeScreen(_currentActiveUI, _teamBuildUIRectTransform);
        _currentActiveUI = _teamBuildUIRectTransform;
    }
    
    
    public void ChangeToSelectCharacterScreen()
    {
        _screenQuickFadeOut.ChangeScreen(_currentActiveUI, _selectCharacterUIRectTransform);
        _currentActiveUI = _selectCharacterUIRectTransform;
    }

    private void Start()
    {
        _homeMenuUI.SetActive(true);
        _characterListUI.SetActive(true);
        _teamBuildUI.SetActive(true);
        _selectCharacterUI.SetActive(true);
        _characterListUI.SetActive(false);
        _teamBuildUI.SetActive(false);
        _selectCharacterUI.SetActive(false);
        
        _homeMenuRectTransform = _homeMenuUI.GetComponent<RectTransform>();
        _characterListRectTransform = _characterListUI.GetComponent<RectTransform>();
        _teamBuildUIRectTransform = _teamBuildUI.GetComponent<RectTransform>();
        _selectCharacterUIRectTransform = _selectCharacterUI.GetComponent<RectTransform>();
        _currentActiveUI = _homeMenuRectTransform;
    }
}
