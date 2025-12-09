using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SupportCardData
{
    [System.Serializable]
    public class CardData
    {
        [SerializeField, Header("ID")]
        private uint _id;
        [SerializeField, Header("名前")]
        private string _cardName;
        [SerializeField, Header("レアリティ")]
        private uint _rarity;
        [SerializeField, Header("体力")]
        private uint _physical;
        [SerializeField, Header("筋力")]
        private uint _power;
        [SerializeField, Header("知力")]
        private uint _intelligence;
        [SerializeField, Header("素早さ")]
        private uint _speed;

        [SerializeField, Header("発生するイベントのID")]
        private uint[] _eventIDArray;

        public uint ID => _id;
        public string CardName => _cardName;
        public uint Rarity => _rarity;
        public uint Physical => _physical;
        public uint Power => _power;
        public uint Intelligence => _intelligence;
        public uint Speed => _speed;
        public uint[] EventIDArray => _eventIDArray;

        /// <summary>
        /// パラメータの初期化
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="charaName">名前</param>
        /// <param name="physi">体力の強化倍率</param>
        /// <param name="pow">筋力の強化倍率</param>
        /// <param name="intelli">知力の強化倍率</param>
        /// <param name="sp">素早さの強化倍率</param>
        public void InitData(string id, string charaName, string physi, string pow, string intelli, string sp, string eventID, string rarity)
        {
            _id = uint.Parse(id);
            _cardName = charaName;
            _physical = uint.Parse(physi);
            _power = uint.Parse(pow);
            _intelligence = uint.Parse(intelli);
            _speed = uint.Parse(sp);
            _rarity = uint.Parse(rarity);

            var events = eventID.Split('|');
            _eventIDArray = new uint[events.Length];

            for (int i = 0; i < _eventIDArray.Length; i++)
            {
                _eventIDArray[i] = uint.Parse(events[i]);
            }
        }
    }

    [CreateAssetMenu(fileName = "SupportCardDataHolder", menuName = "ScriptableObject/SupportCardDataHolder")]
    public class SupportCardDataHolder : ScriptableObject
    {
        [SerializeField, Header("SupportCardのデータリスト")]
        private List<CardData> _dataList = new();

        public List<CardData> DataList => _dataList;

        public void AddData(CardData cardData)
        {
            _dataList.Add(cardData);
        }
    }

    [System.Serializable, CreateAssetMenu(fileName = "SupportCardResource", menuName = "ScriptableObject/SupportCardResource")]
    public class SupportCardSpriteData : ScriptableObject
    {
        [SerializeField] private SpriteData[] _spriteDataArray;

        public SpriteData[] SpriteDataArray => _spriteDataArray;

        /// <summary>
        /// サポートカードの画像データ取得関数
        /// </summary>
        /// <param name="id"> 取得したいサポートカードのID </param>
        /// <returns></returns>
        public Sprite GetCharacterSprite(uint id)
        {
            foreach (var data in _spriteDataArray)
            {
                if (id == data.CardID)
                {
                    return data.CardSprite;
                }
            }

            return null;
        }

        public class SpriteData
        {
            [SerializeField, Header("ID")]
            private uint _cardID = 0;

            [SerializeField, Header("カードのスプライト")]
            private Sprite _cardSprite = null;

            public uint CardID => _cardID;

            public Sprite CardSprite => _cardSprite;
        }
    }

}