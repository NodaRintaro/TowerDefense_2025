using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RankImageData
{
    [SerializeField, SpritePreview] 
    private Sprite _spriteData;

    [SerializeField]
    private RankType _rankType;

    public Sprite SpriteData => _spriteData;
    public RankType RankType => _rankType;
}
