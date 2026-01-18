using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// データのロードを行わないスクリーン変更用の簡易的なフェードアウト
/// </summary>
public class ScreenQuickFadeOut : MonoBehaviour
{
    [SerializeField] private RectTransform currentScreen; // 現在の画面
    [SerializeField] private RectTransform nextScreen;    // 次の画面
    [SerializeField] private CanvasGroup blurOverlay;     // ぼかし用のパネル

    public void ChangeScreen(RectTransform currentScreen, RectTransform nextScreen)
    {
        float screenWidth = Screen.width;

        // 次の画面をあらかじめ右側に待機させる
        nextScreen.transform.position = new Vector2(screenWidth, 0);

        // 1. 現在の画面を左へ
        currentScreen.DOAnchorPos(new Vector2(-screenWidth, 0), 0.5f).SetEase(Ease.InCubic);

        // 2. 次の画面を中央へ
        nextScreen.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic);

        // 3. ぼかしパネルをサッと出して消す
        blurOverlay.alpha = 0;
        blurOverlay.DOFade(1, 0.25f).OnComplete(() => {
            blurOverlay.DOFade(0, 0.25f);
        });
    }
}
