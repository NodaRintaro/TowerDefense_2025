using System;
using UnityEngine;
public class UnitData
{
    public UnitData(TrainedCharacterData trainedCharacterData)
    {
        _roleType = trainedCharacterData.BaseCharacter.RoleType;
        _group = GroupType.Player;
        _id = trainedCharacterData.BaseCharacter.ID;
        _name = trainedCharacterData.BaseCharacter.CharacterName;
        //_cost = trainedCharacterData.Cost;
        _cost = 2f;
        //_rePlaceInterval = trainedCharacterData.RePlaceInterval;
        _rePlaceInterval = 5f;
        _maxHp = trainedCharacterData.Physical;
        _attack = trainedCharacterData.BaseCharacter.Power;
        _magicPower = trainedCharacterData.BaseCharacter.Intelligence;
        //_defense = trainedCharacterData.Defense;
        _defence = 0f;
        //_searchEnemyDistance = trainedCharacterData.SearchEnemyDistance;
        _attackRange = 1f;
        //_actionInterval = trainedCharacterData.ActionInterval;
        _actionInterval = trainedCharacterData.Speed;
        _currentHp = 0f;
    }
    /// <summary>
    /// キャラクターのタイプ
    /// </summary>
    private RoleType _roleType;
    private GroupType _group;
    private uint _id;                         // ユニットID
    private string _name;                    // ユニット名
    private float _cost;
    private float _rePlaceInterval;
    private bool _isDead = false;        // 死亡フラグ
    private float _maxHp;                   // 最大HP
    private float _attack;                  // 攻撃力
    private float _magicPower;              // 魔法力
    private float _defence;                 // 防御力
    private float _attackRange;      // 索敵範囲
    private float _actionInterval;           // 行動間隔
    private float _currentHp;              // 現在のＨＰ
    private float _actionTimer;             // 行動時間

    protected UnitData()
    {
        
    }

    #region Properties
    public RoleType RoleType { get => _roleType; set => _roleType = value; }
    public GroupType Group { get => _group; set => _group = value; }
    public uint ID { get => _id; set => _id = value; }
    public string Name { get => _name; set => _name = value; }
    public float Cost { get => _cost; set => _cost = value; }
    public float RePlaceInterval { get => _rePlaceInterval; set => _rePlaceInterval = value; }
    public bool IsDead { get => _isDead; set => _isDead = value; }
    public float MaxHp { get => _maxHp; set => _maxHp = value; }
    public float Attack { get => _attack; set => _attack = value; }
    public float Defence { get => _defence; set => _defence = value; }
    public float AttackRange { get => _attackRange; set => _attackRange = value; }
    public float ActionInterval { get => _actionInterval; set => _actionInterval = value; }
    public float CurrentHp { get => _currentHp; set => _currentHp = value; }
    public float ActionTimer { get => _actionTimer; set => _actionTimer = value; }
    #endregion
    public enum GroupType
    {
        Player,
        Enemy,
    }
}