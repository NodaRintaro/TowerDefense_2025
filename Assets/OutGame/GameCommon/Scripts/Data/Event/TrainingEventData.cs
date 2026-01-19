using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

[Serializable]
public class TrainingEventData : ITrainingEventData
{
    [Header("イベントのID")]
    [SerializeField] private uint _trainingEventID = 0;

    [Header("イベントの名前")]
    [SerializeField] private string _eventName = null;

    [Header("ノベルイベントのID")]
    [SerializeField] private uint _novelEventID = 0;

    [Header("シナリオの分岐")]
    [SerializeField] private bool _isBrunchScenario = false;

    [Header("シナリオの分岐方法")]
    [SerializeField] private EventBranchWay _branchType = EventBranchWay.None;

    [Header("このイベントが分岐先のイベントの場合の分岐条件")]
    [SerializeField] private EventBranchType _eventBranchType = EventBranchType.None;

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

    public uint EventID => _trainingEventID;
    public string EventName => _eventName;
    public uint NovelEventID => _novelEventID;
    public bool IsBranch => _isBrunchScenario;
    public EventBranchWay BranchType => _branchType;
    public EventBranchType EventBranchType => _eventBranchType;
    public TrainingEventBuffType BuffType => _buffType;
    public int PowerBaseBuff => _powerBaseBuff;
    public int IntelligenceBaseBuff => _intelligenceBaseBuff;
    public int PhysicalBaseBuff => _physicalBaseBuff;
    public int SpeedBaseBuff => _speedBaseBuff;
    public int StaminaBaseBuff => _staminaBaseBuff;
    public uint SkillID => _skillID;
    public uint ItemID => _itemID;

    #if UNITY_EDITOR
    public void SetEventID(uint id) { _trainingEventID = id; }
    public void SetEventName(string eventName) { _eventName = eventName; }
    public void SetNovelEventID(uint id) { _novelEventID = id; }
    public void SetIsBranch(bool isBranch) { _isBrunchScenario = isBranch; }
    public void SetBranchWay(EventBranchWay type) { _branchType = type; }
    public void SetBranchType(EventBranchType eventBranchType) {  _eventBranchType = eventBranchType; }
    public void SetBuffType(TrainingEventBuffType trainingEventBuffType) {  _buffType = trainingEventBuffType; }
    public void SetPowerBuff(int buff) { _powerBaseBuff = buff; }
    public void SetIntelligenceBuff(int buff) { _intelligenceBaseBuff = buff; }
    public void SetPhysicalBuff(int buff) { _physicalBaseBuff = buff; }
    public void SetSpeedBuff(int buff) { _speedBaseBuff = buff; }
    public void SetStaminaBuff(int buff) { _staminaBaseBuff = buff; }
    public void SetSkillID(uint id) { _skillID = id; }
    public void SetItemID(uint id) {_itemID = id; }
    #endif
}
