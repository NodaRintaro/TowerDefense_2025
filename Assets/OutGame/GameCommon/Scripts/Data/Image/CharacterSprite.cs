using System;
using UnityEngine;

[Serializable]
public class CharacterSprite
{
    [SerializeField]
    private Sprite _characterSprite;

    [SerializeField]
    private CharacterSpriteType _spriteType;

    public Sprite SpriteData => _characterSprite;

    public CharacterSpriteType SpriteType => _spriteType;
}