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

    public void StaminaValueChange(uint value)
    {
        float currentValue = value / _staminaVar.maxValue;
        _staminaVar.DOValue(currentValue, _valueMoveSpeed);
    }

    public void SetMaxStamina(uint maxValue) => _staminaVar.maxValue = maxValue;
}
