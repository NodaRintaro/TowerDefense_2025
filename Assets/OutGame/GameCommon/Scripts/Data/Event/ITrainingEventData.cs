public interface ITrainingEventData
{
    public uint EventID { get; }
    public string EventName {  get;  }
    public uint NovelEventID { get; }
    public bool IsBranch { get; }
    public EventBranchWay BranchType { get; }
    public TrainingEventBuffType BuffType { get; }
    public int PowerBaseBuff { get; }
    public int IntelligenceBaseBuff { get; }
    public int PhysicalBaseBuff { get; }
    public int SpeedBaseBuff { get; }
    public int StaminaBaseBuff { get; }
    public uint SkillID { get; }
    public uint ItemID { get; }
}
