using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

namespace CharacterData
{
    /// <summary> キャラクターのベースデータ </summary>
    #region CharacterBaseData
    [Serializable]
    public class CharacterBaseData
    {
        //ステータス
        [SerializeField, Header("ID")]
        protected uint _characterID;
        [SerializeField, Header("名前")]
        protected string _characterName;
        [SerializeField, Header("レア度")]
        protected uint _baseRarity;
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
        public uint BaseRarity => _baseRarity;
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
        /// <param name="rarity">レアリティ</param>
        /// <param name="physi">体力</param>
        /// <param name="pow">筋力</param>
        /// <param name="intelli">知力</param>
        /// <param name="sp">素早さ</param>
        public void InitData(uint id, string charaName, uint rarity, uint physi, uint pow, uint intelli, uint sp, string role, uint cost)
        {
            _characterID = id;
            _characterName = charaName;
            _baseRarity = rarity;
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
            _baseRarity = baseData.BaseRarity;
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
    #endregion

    /// <summary> トレーニング中のキャラクターデータ </summary>
    #region TrainingCharacterData
    [System.Serializable]
    public class TrainingCharacterData : CharacterBaseData
    {
        [Inject]
        public TrainingCharacterData(CharacterBaseData data)
        {
            SetBaseData(data);
        }

        //各種パラメータの増加値
        [SerializeField, Header("体力の増加値")]
        private ReactiveProperty<uint> _currentPhysicalBuff = new ReactiveProperty<uint>(0);
        [SerializeField, Header("攻撃力の増加値")]
        private ReactiveProperty<uint> _currentPowerBuff = new ReactiveProperty<uint>(0);
        [SerializeField, Header("知力の増加値")]
        private ReactiveProperty<uint> _currentIntelligenceBuff = new ReactiveProperty<uint>(0);
        [SerializeField, Header("素早さの増加値")]
        private ReactiveProperty<uint> _currentSpeedBuff = new ReactiveProperty<uint>(0);

        [SerializeField, Header("スタミナの最大値")]
        private uint _maxStamina;

        [SerializeField, Header("キャラクターのスタミナ")]
        private uint _currentStamina;

        #region 各種パラメータのベースパラメータと強化値の合計値
        public uint TotalPhysical => _currentPhysicalBuff.Value + BasePhysical;
        public uint TotalPower => _currentPowerBuff.Value + BasePower;
        public uint TotalIntelligence => _currentIntelligenceBuff.Value + BaseIntelligence;
        public uint TotalSpeed => _currentSpeedBuff.Value + BaseSpeed;
        #endregion

        #region 各種パラメータの参照用プロパティ
        public ReactiveProperty<uint> CurrentPhysicalBuff => _currentPhysicalBuff;
        public ReactiveProperty<uint> CurrentPowerBuff => _currentPowerBuff;
        public ReactiveProperty<uint> CurrentIntelligenceBuff => _currentIntelligenceBuff;
        public ReactiveProperty<uint> CurrentSpeedBuff => _currentSpeedBuff;
        public uint MaxStamina => _maxStamina;
        public uint CurrentStamina => _currentStamina;
        #endregion

        #region 各種パラメータの増加処理
        public void AddCurrentPhysical(uint physical) => _currentPhysicalBuff.Value += physical;
        public void AddCurrentPower(uint power) => _currentPowerBuff.Value += power;
        public void AddCurrentIntelligence(uint intelligence) => _currentIntelligenceBuff.Value += intelligence;
        public void AddCurrentSpeed(uint speed) => _currentSpeedBuff.Value += speed;
        public void UseStamina(uint stamina) => _currentStamina -= stamina;
        public void TakeBreak(uint stamina)
        {
            if (_currentStamina + stamina > _maxStamina)
            {
                _currentStamina = _maxStamina;
            }
            else
            {
                _currentStamina += stamina;
            }
        }
        #endregion

        public void SetMaxStamina(uint stamina) => _maxStamina = stamina;
    }


    /// <summary>
    /// キャラクターの画像データをまとめて管理するScriptableObjectClass
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterSprite", menuName = "ScriptableObject/CharacterSprite")]
    public class CharacterSpriteHolder : ScriptableObject
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

        [Serializable]
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

            [SpritePreview, Header("キャラクターの立ち絵")]
            public Sprite SpriteData;
        }
    }
    #endregion

    /// <summary> トレーニング済みのキャラクターデータ </summary>
    #region TrainedCharacterData
    [Serializable]
    public class TrainedCharacterData : CharacterBaseData
    {
        [SerializeField, Header("トレーニング後のキャラクターID")]
        private int _trainiedID;

        [SerializeField, Header("トレーニング後のキャラクターランク")]
        private RankType _rankType;

        [SerializeField, Header("体力増加値")]
        private uint _addPhysical;
        [SerializeField, Header("筋力増加値")]
        private uint _addPower;
        [SerializeField, Header("知力増加値")]
        private uint _addIntelligence;
        [SerializeField, Header("素早さ増加値")]
        private uint _addSpeed;

        /// <summary> トレーニング後のキャラクターデータID </summary>
        public int TrainedCharacterID => _trainiedID;

        #region 増加値の参照用プロパティ
        public uint AddPhysical => _addPhysical;
        public uint AddPower => _addPower;
        public uint AddIntelligence => _addIntelligence;
        public uint AddSpeed => _addSpeed;
        #endregion

        #region 合計値の参照プロパティ
        public uint TotalPhysical => _addPhysical + _basePhysical;
        public uint TotalPower => _addPower + _basePower;
        public uint TotalIntelligence => _baseIntelligence;
        public uint TotalSpeed => _addSpeed + _baseSpeed;
        #endregion

        public void SetCharacterTrainedParameterData(int newID, uint setPhysi, uint setPow, uint setInt, uint setSp)
        {
            _trainiedID = newID;
            _addPhysical = setPhysi;
            _addPower = setPow;
            _addIntelligence = setInt;
            _addSpeed = setSp;
        }

        public void SetCharacterRank(RankType rankType)
        {
            _rankType = rankType;
        }
    }
    #endregion
}

