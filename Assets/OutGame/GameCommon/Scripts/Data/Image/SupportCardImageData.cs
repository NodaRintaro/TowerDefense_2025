using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サポートカードの画像データ
/// </summary>
[Serializable]
public class SupportCardImageData
{
    [SerializeField]
    private Sprite _spriteData;

    [SerializeField]
    private uint _id;

    public Sprite SpriteData => _spriteData;
    public uint ID => _id;
}
