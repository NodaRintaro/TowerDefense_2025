using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SlideInAnimation
{
    /// <summary>
    /// 指定されたGameObjectを画面右からスライドさせて指定されたX座標で停止させます。
    /// </summary>
    public async static UniTask SlideInGameObject(GameObject targetGameObject, float centerPosX, float duration)
    {
        if (targetGameObject == null)
        {
            Debug.LogWarning("Target GameObject is null for SlideInGameObjectFromRight.");
            return;
        }

        RectTransform rectTransform = targetGameObject.GetComponent<RectTransform>();
        bool isCompleteTask = false;
        if (rectTransform != null)
        {
            // 初期位置を画面右外に設定
            rectTransform.anchoredPosition = new Vector2(Screen.width, rectTransform.anchoredPosition.y);
            // 指定されたX座標までスライド
            rectTransform.DOAnchorPosX(centerPosX, duration).SetEase(Ease.OutQuad).OnComplete(() => isCompleteTask = true);
            await UniTask.WaitUntil(() => isCompleteTask); 
        }
        else
        {

            targetGameObject.transform.localPosition = new Vector3(Screen.width, targetGameObject.transform.localPosition.y, targetGameObject.transform.localPosition.z);
            // 指定されたX座標までスライド
            targetGameObject.transform.DOLocalMoveX(centerPosX, duration).SetEase(Ease.OutQuad).OnComplete(() => isCompleteTask = true);
            await UniTask.WaitUntil(() => isCompleteTask);
        }
    }

    /// <summary>
    /// 指定されたGameObjectを現在位置から画面左へスライドアウトさせます。
    /// </summary>
    public async static UniTask SlideOutGameObject(GameObject targetGameObject, float exitPosX, float duration)
    {
        if (targetGameObject == null)
        {
            Debug.LogWarning("Target GameObject is null for SlideOutGameObjectToLeft.");
            return;
        }

        bool isCompleteTask = false;
        RectTransform rectTransform = targetGameObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.DOAnchorPosX(exitPosX, duration).SetEase(Ease.InQuad).OnComplete(() => isCompleteTask = true);
            await UniTask.WaitUntil(() => isCompleteTask);
        }
        else
        {
            targetGameObject.transform.DOLocalMoveX(exitPosX, duration).SetEase(Ease.InQuad).OnComplete(() => isCompleteTask = true);
            await UniTask.WaitUntil(() => isCompleteTask);
        }
    }

    /// <summary>
    /// 指定されたGameObjectを画面上からスライドさせて指定されたY座標で停止させます。
    /// </summary>
    public async static UniTask SlideInGameObjectFromTop(GameObject targetGameObject, float centerPosY, float duration)
    {
        if (targetGameObject == null)
        {
            Debug.LogWarning("Target GameObject is null for SlideInGameObjectFromTop.");
            return;
        }

        bool isCompleteTask = false;
        RectTransform rectTransform = targetGameObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 初期位置を画面上外（Screen.height）に設定
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, Screen.height);
            // 指定されたY座標までスライド
            rectTransform.DOAnchorPosY(centerPosY, duration).SetEase(Ease.OutQuad).OnComplete(() => isCompleteTask = true);
            await UniTask.WaitUntil(() => isCompleteTask);
        }
        else
        {
            // 3Dオブジェクト等の場合
            targetGameObject.transform.localPosition = new Vector3(targetGameObject.transform.localPosition.x, Screen.height, targetGameObject.transform.localPosition.z);
            // 指定されたY座標までスライド
            targetGameObject.transform.DOLocalMoveY(centerPosY, duration).SetEase(Ease.OutQuad).OnComplete(() => isCompleteTask = true);
            await UniTask.WaitUntil(() => isCompleteTask);
        }
    }

    /// <summary>
    /// 指定されたGameObjectを現在位置から画面上へスライドアウトさせます。
    /// </summary>
    public async static UniTask SlideOutGameObjectToTop(GameObject targetGameObject, float exitPosY, float duration)
    {
        if (targetGameObject == null)
        {
            Debug.LogWarning("Target GameObject is null for SlideOutGameObjectToTop.");
            return;
        }

        bool isCompleteTask = false;
        RectTransform rectTransform = targetGameObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 指定されたY座標（画面外など）へスライド
            rectTransform.DOAnchorPosY(exitPosY, duration).SetEase(Ease.InQuad).OnComplete(() => isCompleteTask = true);
            await UniTask.WaitUntil(() => isCompleteTask);
        }
        else
        {
            targetGameObject.transform.DOLocalMoveY(exitPosY, duration).SetEase(Ease.InQuad).OnComplete(() => isCompleteTask = true);
            await UniTask.WaitUntil(() => isCompleteTask);
        }
    }
}
