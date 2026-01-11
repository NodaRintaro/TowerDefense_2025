using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPagePresenter
{
    /// <summary> 現在のページからほかのページへ移行するイベントをボタンに登録する処理 </summary>
    public void SetOnClickTurnPageButtonEvent();
}
