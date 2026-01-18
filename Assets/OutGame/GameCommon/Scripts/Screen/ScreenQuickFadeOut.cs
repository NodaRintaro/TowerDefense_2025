using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// データのロードを行わないスクリーン変更用の簡易的なフェードアウト
/// </summary>
public class ScreenQuickFadeOut : MonoBehaviour
{
    [SerializeField] private Image blurOverlay;     // ぼかし用のパネル

    public void ChangeScreen(RectTransform currentScreen, RectTransform nextScreen)
    {
        float screenWidth = Screen.width;

        // 次の画面をあらかじめ右側に待機させる
        currentScreen.gameObject.SetActive(true);
        nextScreen.gameObject.SetActive(true);
        nextScreen.anchoredPosition = new Vector2(screenWidth, 0);

        // 1. 現在の画面を左へ
        currentScreen.DOAnchorPos(new Vector2(-screenWidth, 0), 0.5f).SetEase(Ease.InCubic).OnComplete(()=> currentScreen.gameObject.SetActive(false));

        // 2. 次の画面を中央へ
        nextScreen.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.InCubic);

        // 3. ぼかしパネルをサッと出して消す
        blurOverlay.gameObject.SetActive(true);
        blurOverlay.DOFade(0f, 0f);
        blurOverlay.DOFade(1, 0.5f).OnComplete(() => {
            blurOverlay.DOFade(0, 0.5f).OnComplete(() =>
            {
                blurOverlay.gameObject.SetActive(false);
            });
        });
    }
    
    
}
