using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

[Serializable]
public class TrainingEventData
{
    [Header("トレーニングイベントのID")]
    [SerializeField] private uint _trainingEventID = 0;

    [Header("イベントの名前")]
    [SerializeField] private string _eventName = null;

    [Header("ノベルイベントのID")]
    [SerializeField] private uint _novelEventID = 0;

    [Header("各種パラメータの強化値")]
    [SerializeField] private int _powerBaseBuff = 0;
    [SerializeField] private int _intelligenceBaseBuff = 0;
    [SerializeField] private int _physicalBaseBuff = 0;
    [SerializeField] private int _speedBaseBuff = 0;

    [Header("スタミナの変動値")]
    [SerializeField] private int _staminaBaseBuff = 0;

    [Header("獲得するスキルのID")]
    [SerializeField] private uint _skillID = 0;

    [Header("獲得するアイテムのID")]
    [SerializeField] private uint _itemID = 0;

    public uint TrainingEventID => _trainingEventID;
    public string EventName => _eventName;
    public uint NovelEventID => _novelEventID;
    public int PowerBaseBuff => _powerBaseBuff;
    public int IntelligenceBaseBuff => _intelligenceBaseBuff;
    public int PhysicalBaseBuff => _physicalBaseBuff;
    public int SpeedBaseBuff => _speedBaseBuff;
    public int StaminaBaseBuff => _staminaBaseBuff;
    public uint SkillID => _skillID;
    public uint ItemID => _itemID;

    public void InitData(uint trainingEventID, string eventName, uint eventID, int powerBuff, int intelligenceBuff, int physicalBuff, int speedBuff, int staminaBuff, uint skillID, uint itemID)
    {
        _trainingEventID = trainingEventID;
        _eventName = eventName;
         _novelEventID = eventID;
        _powerBaseBuff = powerBuff;
        _intelligenceBaseBuff = intelligenceBuff;
        _physicalBaseBuff = physicalBuff;
        _speedBaseBuff = speedBuff;
        _staminaBaseBuff = staminaBuff;
        _skillID = skillID;
        _itemID = itemID;
    }
}
