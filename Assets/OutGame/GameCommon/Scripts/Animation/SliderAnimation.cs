using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAnimation 
{
    private const float _duration = 0.2f;

    /// <summary> SliderのValueを動かす際のAnimation </summary>
    public async static UniTask SliderValueMoveAnimation(Slider slider, uint targetValue)
    {
        slider.DOValue(targetValue, _duration).SetEase(Ease.OutQuad);
        await UniTask.WaitUntil(() => slider.value == targetValue);
    }
}
