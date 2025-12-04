using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    /// <summary> ViewのGetter </summary>
    public IViewData View { get; }

    /// <summary> Viewの初期化関数 </summary>
    public void InitView();

    /// <summary> Eventをセットする関数 </summary>
    public void SetEvents();
}
