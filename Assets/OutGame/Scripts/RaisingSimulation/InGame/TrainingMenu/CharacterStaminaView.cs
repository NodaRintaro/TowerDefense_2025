using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class CharacterStaminaView
{
    [SerializeField] private Slider _staminaVar;

    [SerializeField] private float _valueMoveSpeed = 1.0f;

    /// <summary> スタミナバーの値を変動させる </summary>
    public void StaminaValueChange(uint value) => _staminaVar.DOValue(_staminaVar.maxValue < value ? _staminaVar.maxValue : value, _valueMoveSpeed);

    /// <summary> スタミナバーの最大値を変える </summary>
    public void SetMaxStamina(uint maxValue) => _staminaVar.maxValue = maxValue;

    /// <summary> スタミナをゼロにする </summary>
    public void ValueZero() => _staminaVar.value = 0;
}
