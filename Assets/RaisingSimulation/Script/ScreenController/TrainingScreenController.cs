using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingScreenController : MonoBehaviour
{
    /// <summary> ScreenTypeが変化した際に通知するための変数 </summary>
    private event Action<ScreenType> _onChangedScreenType;

    [SerializeField, Header("現在の表示画面")] 
    ScreenType _currentScreenType = ScreenType.Empty;

    private void Awake()
    {
        //シーン遷移後に何もなければキャラクター選択画面へ移行
        if(_currentScreenType == ScreenType.Empty)
            ChangeScreen(ScreenType.CharacterPick);
    }

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
    [InspectorName("表示画面が選択されていません")] Empty,
    [InspectorName("キャラクター選択画面")] CharacterPick,
    [InspectorName("キャラクター育成画面")] TrainingMenu,
    [InspectorName("キャラクター育成イベント画面")] TrainingEvent
}
