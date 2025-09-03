using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingScreenChanger : MonoBehaviour
{
    /// <summary> ScreenTypeが変化した際に通知するための変数 </summary>
    private event Action<ScreenType> _onChangedScreenType;

    [SerializeField, Header("現在の表示画面")] 
    ScreenType _currentScreenType;

    public ScreenType CurrentScreenType => _currentScreenType;

    public event Action<ScreenType> OnChangedScreenType
    {
        add { _onChangedScreenType += value; }
        remove { _onChangedScreenType -= value; }
    }

    /// <summary> ScreenTypeを変える関数 </summary>
    public void ChangeScreen(ScreenType nextScreenType)
    {
        if(nextScreenType == _currentScreenType) return;

        _currentScreenType = nextScreenType;

        _onChangedScreenType?.Invoke(nextScreenType);
    }
}

public enum ScreenType
{
    CharacterPick,
    TrainingMenu,
    TrainingEvent
}
