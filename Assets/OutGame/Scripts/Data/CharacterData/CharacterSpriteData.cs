using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSprite", menuName = "ScriptableObject/CharacterSprite")]
public class CharacterSpriteData : ScriptableObject
{
    [SerializeField, Header("ID")]
    private uint _characterID = 0;

    [SerializeField, Header("キャラクターの画像データ")]
    CharacterSprite[] _chracterSpriteData;

    public uint CharacterID => _characterID;

    public CharacterSprite[] ChracterSpriteDatas => _chracterSpriteData;

    public Sprite GetCharacterSprite(CharacterSpriteType spriteType)
    {
        foreach (var data in ChracterSpriteDatas)
        {
            if (spriteType == data.SpriteType)
            {
                return data.SpriteData;
            }
        }

        return null;
    }

    [System.Serializable]
    public struct CharacterSprite
    {
        [Header("登録してあるSpriteのタイプ")]
        public CharacterSpriteType SpriteType;

        [Header("キャラクターの立ち絵")] 
        public Sprite SpriteData;
    }
}