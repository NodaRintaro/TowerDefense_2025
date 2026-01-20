using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] private GameObject _stageUI;
    [SerializeField] private CanvasGroup _stageCanvasGroup;
    [SerializeField] private TMP_Text _stageNameText;
    [SerializeField] private List<StageData> _stageDataList = new List<StageData>();
    
    public void StageDataView(int stageId)
    {
        foreach (var stageData in _stageDataList)
        {
            if (stageData.stageID == stageId)
            {
                _stageNameText.text = stageData.stageName;
                
                StageDataLoader.SetStage(stageData);
            }
        }

        ShowStageUI();
    }

    private void Start()
    {
        _stageUI.SetActive(false);
    }

    private void ShowStageUI()
    {
        _stageUI.SetActive(true);
        Image stageUI = _stageUI.GetComponent<Image>();
        _stageCanvasGroup.DOFade(0f, 0f);
        _stageCanvasGroup.DOFade(1f, 1f).SetEase(Ease.InQuad);
    }
    
    public void CloseStageUI()
    {
        _stageCanvasGroup.DOFade(1f, 0f);
        _stageCanvasGroup.DOFade(0f, 1f).SetEase(Ease.InQuad)
            .OnComplete(() => _stageUI.SetActive(false));
    }
    
    public void SceneChangeToGameScene()
    {
        SceneChanger.SceneChange("InGame");
    }
}