using UnityEngine;

[System.Serializable]
public class SkillData : ISkill
{
    [SerializeField, Header("ID")] private uint _skillID;
    [SerializeField, Header("名前")] private string _skillName;
    [SerializeField, TextArea, Header("説明文")] private string _skillDescription;
    [SerializeField, Header("アイコン")] private Sprite _skillIcon;
    [SerializeField, Header("プレハブ")] private GameObject _skillPrefab;
    [SerializeField, Header("自動発動かどうか")] private bool _isUseAutomatic;
    [SerializeField, Header("持続タイプ")] private DurationType _durationType;
    [SerializeField, Header("持続時間")] private float _duration;
    [SerializeField, Header("クールタイムタイプ")] private CoolTimeRecoveryType _coolTimeRecoveryType;
    [SerializeField, Header("クールタイム")] private float _coolTime;
    [SerializeField, Header("消費コスト")] private int _consumptionCost;

    private int[,] _skillRange = new int[11, 11];

    public uint SkillID => _skillID;
    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;
    public GameObject SkillPrefab => _skillPrefab;
    public bool IsUseAutomatic => _isUseAutomatic;
    public DurationType DurationType => _durationType;
    public float Duration => _duration;
    public CoolTimeRecoveryType CoolTimeRecovery => _coolTimeRecoveryType;
    public float CoolTime => _coolTime;
    public int ConsumptionCost => _consumptionCost;
    public int[,] SkillRange => _skillRange;

    /// <summary>
    /// パラメータの初期化関数
    /// </summary>
    /// <param name="skillID">ID</param>
    /// <param name="skillName">名前</param>
    /// <param name="skillDescription">説明文</param>
    /// <param name="skillPrefab">プレハブ</param>
    /// <param name="isUseAutomatic">自動発動かどうか</param>
    /// <param name="durationType">持続タイプ</param>
    /// <param name="duration">持続時間</param>
    /// <param name="coolTimeRecoveryType">クールタイムタイプ</param>
    /// <param name="cooltTime">クールタイム</param>
    /// <param name="consumptionCost">消費コスト</param>
    public void InitData(uint skillID, string skillName, string skillDescription,
                    Sprite icon, GameObject skillPrefab, bool isUseAutomatic,
                    DurationType durationType,float duration , CoolTimeRecoveryType coolTimeRecoveryType, float cooltTime,
                    int consumptionCost = 0)
    {
        _skillID = skillID;
        _skillName = skillName;
        _skillDescription = skillDescription;
        _skillIcon = icon;
        _skillPrefab = skillPrefab;
        _isUseAutomatic = isUseAutomatic;
        _durationType = durationType;
        _duration = duration;
        _coolTimeRecoveryType = coolTimeRecoveryType;
        _coolTime = cooltTime;
        _consumptionCost = consumptionCost;
    }

    /// <summary>
    /// スキル範囲のセットを行う
    /// </summary>
    /// <param name="skillRange">スキル範囲</param>
    public void SetSkillRange(int[,] skillRange)
    {
        _skillRange = skillRange;
    }
}
