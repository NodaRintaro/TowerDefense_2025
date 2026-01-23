using System;
using TMPro;
using UnityEngine;

[Serializable]
public class TrainingRaidCountDownView
{
    [SerializeField] private TMP_Text _countDownText;

    public void CountDown(int count)
    {
        _countDownText.text = count.ToString();
        TextAnimation.ScalePulse(_countDownText);
    }
}
