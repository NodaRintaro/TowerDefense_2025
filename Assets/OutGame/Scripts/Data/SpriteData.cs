using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteData
{
    public abstract class SpriteBaseData
    {
        /// <summary> 画像データ </summary>
        [SerializeField, SpritePreview] private Sprite _sprit;

        public Sprite Sprite => _sprit;
    }

    [Serializable]
    public class CharacterSprite : SpriteBaseData
    {
        [SerializeField] private uint _characterID;

        public uint CharacterID => _characterID;
    }

    [Serializable]
    public class SupportCardSprite : SpriteBaseData
    {
        [SerializeField] private uint _cardID;

        public uint CardID => _cardID;
    }

    [Serializable]
    public class RankSprite : SpriteBaseData
    {
        [SerializeField] private RankType _rank;

        public RankType Rank => _rank;
    }
}