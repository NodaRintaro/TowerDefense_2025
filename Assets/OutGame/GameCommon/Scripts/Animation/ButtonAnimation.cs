using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// Buttonアニメーション全般の管理クラス
/// </summary>
public class ButtonAnimation
{
    private const float _duration = 0.2f;
    private const float _hoverScale = 1.1f;

    /// <summary> マウスカーソルがボタンに重なったときのアニメーションをつける処理 </summary>
    public static void SetupPointerEnterAnimationEvents(Button btn)
    {
        var trigger = btn.gameObject.GetComponent<EventTrigger>() ?? btn.gameObject.AddComponent<EventTrigger>();
        var rect = btn.GetComponent<RectTransform>();
        var defaultScale = rect.localScale;

        // PointerEnterイベントの登録
        AddEvent(trigger, EventTriggerType.PointerEnter, _ => {
            rect.DOScale(defaultScale * _hoverScale, _duration).SetEase(Ease.OutCubic).SetLink(btn.gameObject);
        });

        // PointerExitイベントの登録
        AddEvent(trigger, EventTriggerType.PointerExit, _ => {
            rect.DOScale(defaultScale, _duration).SetEase(Ease.OutCubic).SetLink(btn.gameObject);
        });
    }

    public static void RemoveAnimationEvent(Button button)
    {
        var trigger = button.GetComponent<EventTrigger>();
        if (trigger != null)
        {
            trigger.triggers.Clear(); // イベントを削除
        }

        // DOTweenのアニメーションを停止させ、スケールをリセット
        var rect = button.GetComponent<RectTransform>();
        rect.DOKill();
        rect.localScale = Vector3.one;
    }

    /// <summary> ButtonにEventを登録する処理 </summary>
    private static void AddEvent(EventTrigger trigger, EventTriggerType type, System.Action<BaseEventData> action)
    {
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(action));
        trigger.triggers.Add(entry);
    }
}