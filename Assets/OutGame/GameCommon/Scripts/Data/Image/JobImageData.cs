using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの役職の画像データ保管クラス
/// </summary>
[Serializable]
public class JobImageData
{
    [SerializeField]
    private Sprite _spriteData;

    [SerializeField]
    private JobType _jobType;

    public Sprite SpriteData => _spriteData;
    public JobType JobData => _jobType;
}
