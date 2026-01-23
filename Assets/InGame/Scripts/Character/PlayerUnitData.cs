
public class PlayerUnitData : UnitData
{
    protected JobType _jobType;             // ユニットのタイプ
    protected uint _id;                       // ユニットID
    protected uint _cost;                    // ユニットを出すのに必要なコスト
    protected float _rePlaceInterval;         // 再出撃に必要な時間
    protected float _rePlaceTimer;            // 再出撃用タイマー
    
    public PlayerUnitData(TowerDefenseCharacterData trainedCharacterData)
    {
        _jobType = trainedCharacterData.CharacterRole;
        _group = GroupType.Player;
        _id = trainedCharacterData.CharacterID;
        _name = trainedCharacterData.CharacterName;
        _cost = trainedCharacterData.Cost;
        //_rePlaceInterval = trainedCharacterData.RePlaceInterval;
        _rePlaceInterval = 5f;
        _maxHp = trainedCharacterData.TotalPhysical;
        _attack = trainedCharacterData.TotalPower;
        _magicPower = trainedCharacterData.TotalIntelligence;
        _actionInterval = 1.25f + 80 / trainedCharacterData.TotalSpeed;
        //_searchEnemyDistance = trainedCharacterData.SearchEnemyDistance;
        _attackRange = 1f;
        _currentHp = _maxHp;
        _jobType = trainedCharacterData.CharacterRole;
    }
    public float RePlaceTimer { get => _rePlaceTimer; set => _rePlaceTimer = value; }
    public JobType JobType { get => _jobType; set => _jobType = value; }
    public uint Cost { get => _cost; set => _cost = value; }
    public float RePlaceInterval { get => _rePlaceInterval; set => _rePlaceInterval = value; }
    public uint ID { get => _id; set => _id = value; }
}
