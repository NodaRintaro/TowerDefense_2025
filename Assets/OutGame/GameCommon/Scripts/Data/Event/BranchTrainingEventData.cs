using System;
using UnityEngine;

[Serializable]
public class BranchTrainingEventData : ITrainingEventData
{
    [Header("分岐元イベントのID")]
    [SerializeField] private uint _branchBaseEventID = 0;

    [Header("イベントの名前")]
    [SerializeField] private string _eventName = null;

    [Header("ノベルイベントのID")]
    [SerializeField] private uint _novelEventID = 0;

    [Header("このシナリオまで分岐する方法")]
    [SerializeField] private EventBranchType _trainingEventBranchType;

    [Header("シナリオが分岐するかの判定")]
    [SerializeField] private bool _isBrunchScenario = false;

    [Header("このシナリオの分岐方法")]
    [SerializeField] private EventBranchWay _branchWay = EventBranchWay.None;

    [Header("Buffの種類")]
    [SerializeField] private TrainingEventBuffType _buffType = TrainingEventBuffType.None;

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

    public uint EventID => _branchBaseEventID;
    public string EventName => _eventName;
    public uint NovelEventID => _novelEventID;
    public EventBranchType TrainingEventBranchType => _trainingEventBranchType;
    public bool IsBranch => _isBrunchScenario;
    public EventBranchWay BranchType => _branchWay;
    public TrainingEventBuffType BuffType => _buffType;
    public int PowerBaseBuff => _powerBaseBuff;
    public int IntelligenceBaseBuff => _intelligenceBaseBuff;
    public int PhysicalBaseBuff => _physicalBaseBuff;
    public int SpeedBaseBuff => _speedBaseBuff;
    public int StaminaBaseBuff => _staminaBaseBuff;
    public uint SkillID => _skillID;
    public uint ItemID => _itemID;

#if UNITY_EDITOR
    public void SetEventID(uint id) { _branchBaseEventID = id; }
    public void SetEventName(string eventName) { _eventName = eventName; }
    public void SetNovelEventID(uint id) { _novelEventID = id; }
    public void SetTrainingEventBranchType(EventBranchType trainingEventBranchType) { _trainingEventBranchType = trainingEventBranchType; }
    public void SetIsBranch(bool isBranch) { _isBrunchScenario = isBranch; }
    public void SetBranchType(EventBranchWay type) { _branchWay = type; }
    public void SetBuffType(TrainingEventBuffType trainingEventBuffType) { _buffType = trainingEventBuffType; }
    public void SetPowerBuff(int buff) { _powerBaseBuff = buff; }
    public void SetIntelligenceBuff(int buff) { _intelligenceBaseBuff = buff; }
    public void SetPhysicalBuff(int buff) { _physicalBaseBuff = buff; }
    public void SetSpeedBuff(int buff) { _speedBaseBuff = buff; }
    public void SetStaminaBuff(int buff) { _staminaBaseBuff = buff; }
    public void SetSkillID(uint id) { _skillID = id; }
    public void SetItemID(uint id) { _itemID = id; }
#endif
}
