using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace CharacterData
{
    /// <summary> キャラクターのベースデータ </summary>
    [System.Serializable]
    public class CharacterBaseData
    {
        //ステータス
        [SerializeField, Header("ID")]
        protected uint _characterID;
        [SerializeField, Header("名前")]
        protected string _characterName;
        [SerializeField, Header("体力")]
        protected uint _basePhysical;
        [SerializeField, Header("筋力")]
        protected uint _basePower;
        [SerializeField, Header("知力")]
        protected uint _baseIntelligence;
        [SerializeField, Header("素早さ")]
        protected uint _baseSpeed;
        [SerializeField, Header("戦闘スタイル")]
        protected RoleType _roleType;
        [SerializeField, Header("コスト")]
        protected uint _cost;
        public uint CharacterID => _characterID;
        public string CharacterName => _characterName;
        public uint BasePhysical => _basePhysical;
        public uint BasePower => _basePower;
        public uint BaseIntelligence => _baseIntelligence;
        public uint BaseSpeed => _baseSpeed;
        public RoleType CharacterRole => _roleType;
        public uint Cost => _cost;

        /// <summary>
        /// パラメータの初期化関数
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="charaName">名前</param>
        /// <param name="physi">体力</param>
        /// <param name="pow">筋力</param>
        /// <param name="intelli">知力</param>
        /// <param name="sp">素早さ</param>
        public void InitData(uint id, string charaName, uint physi, uint pow, uint intelli, uint sp, string role, uint cost)
        {
            _characterID = id;
            _characterName = charaName;
            _basePhysical = physi;
            _basePower = pow;
            _baseIntelligence = intelli;
            _baseSpeed = sp;
            SetCharacterRole(role);
            _cost = cost;
        }

        /// <summary>
        /// ベースデータの登録関数
        /// </summary>
        /// <param name="baseData"></param>
        public void SetBaseData(CharacterBaseData baseData)
        {
            _characterID = baseData.CharacterID;
            _characterName = baseData.CharacterName;
            _basePhysical = baseData.BasePhysical;
            _basePower = baseData.BasePower;
            _baseIntelligence = baseData.BaseIntelligence;
            _baseSpeed = baseData.BaseSpeed;
            _roleType = baseData.CharacterRole;
            _cost = baseData.Cost;
        }

        protected void SetCharacterRole(string roleType)
        {
            switch (roleType)
            {
                case "Attacker":
                    _roleType = RoleType.Attacker;
                    break;
                case "Tank":
                    _roleType = RoleType.Tank;
                    break;
                case "Magic":
                    _roleType = RoleType.Magic;
                    break;
                case "Sniper":
                    _roleType = RoleType.Sniper;
                    break;
                case "Healer":
                    _roleType = RoleType.Healer;
                    break;
                case "Supporter":
                    _roleType = RoleType.Supporter;
                    break;
                case "Special":
                    _roleType = RoleType.Special;
                    break;
                default:
                    _roleType = RoleType.Null;
                    break;
            }
        }
    }

    [CreateAssetMenu(fileName = "CharacterDataList", menuName = "ScriptableObject/CharacterDataList")]
    public class CharacterDataHolder : ScriptableObject
    {
        [SerializeField, Header("キャラクターのデータリスト")]
        private List<CharacterBaseData> _dataList = new();

        public List<CharacterBaseData> CharacterInformationDataList => _dataList;

        public void AddData(CharacterBaseData characterData)
        {
            _dataList.Add(characterData);
        }

        public CharacterBaseData GetData(uint id)
        {
            foreach (var item in _dataList)
            {
                if (item.CharacterID == id)
                {
                    return item;
                }
            }
            return null;
        }
    }

    [CreateAssetMenu(fileName = "CharacterSprite", menuName = "ScriptableObject/CharacterSprite")]
    public class CharacterSpriteData : ScriptableObject
    {
        [SerializeField] private SpriteData[] _spriteDataArray;

        public SpriteData[] SpriteDataArray => _spriteDataArray;

        /// <summary>
        /// キャラクターの画像データ取得関数
        /// </summary>
        /// <param name="id"> 取得したいキャラクターのID </param>
        /// <param name="characterSpriteType"> 取得したいキャラクターの画像タイプ </param>
        /// <returns></returns>
        public Sprite GetCharacterSprite(uint id, CharacterSpriteType characterSpriteType)
        {
            foreach (var data in _spriteDataArray)
            {
                if (id == data.CharacterID)
                {
                    return data.GetSprite(characterSpriteType);
                }
            }

            return null;
        }

        public class SpriteData
        {
            [SerializeField, Header("ID")]
            private uint _characterID = 0;

            [SerializeField, Header("キャラクターの画像データ")]
            CharacterSprite[] _chracterSpriteArray;

            public uint CharacterID => _characterID;

            public CharacterSprite[] ChracterSpriteArray => _chracterSpriteArray;

            public Sprite GetSprite(CharacterSpriteType characterSpriteType)
            {
                foreach (var data in _chracterSpriteArray)
                {
                    if (characterSpriteType == data.SpriteType)
                    {
                        return data.SpriteData;
                    }
                }

                return null;
            }
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
}

