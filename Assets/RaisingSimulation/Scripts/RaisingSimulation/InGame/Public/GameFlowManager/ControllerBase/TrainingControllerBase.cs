using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public abstract class TrainingControllerBase
{
    //LifeTimeScope
    protected TrainingInGameLifeTimeScope _lifeTimeScope;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public abstract void Init();
}
