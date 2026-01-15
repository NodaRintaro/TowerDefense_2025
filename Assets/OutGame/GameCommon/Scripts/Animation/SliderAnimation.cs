using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAnimation 
{
    /// <summary> SliderのValueを動かす際のAnimation </summary>
    public async static UniTask SliderValueMoveAnimation(Slider slider, uint targetValue, float duration)
    {
        slider.DOValue(targetValue, duration).SetEase(Ease.OutQuad);
        await UniTask.WaitUntil(() => slider.value == targetValue);
    }
}
