using TrainedData;

public class PlayerUnitData : UnitData
{
    protected RoleType _roleType;             // ユニットのタイプ
    protected uint _id;                       // ユニットID
    protected float _cost;                    // ユニットを出すのに必要なコスト
    protected float _rePlaceInterval;         // 再出撃に必要な時間
    protected float _rePlaceTimer;            // 再出撃用タイマー
    
    public PlayerUnitData(TrainedCharacterData trainedCharacterData)
    {
        _roleType = trainedCharacterData.CharacterRole;
        _group = GroupType.Player;
        _id = trainedCharacterData.CharacterID;
        _name = trainedCharacterData.CharacterName;
        //_cost = trainedCharacterData.Cost;
        _cost = 2f;
        //_rePlaceInterval = trainedCharacterData.RePlaceInterval;
        _rePlaceInterval = 5f;
        _maxHp = trainedCharacterData.AddPhysical;
        _attack = trainedCharacterData.BasePower;
        _magicPower = trainedCharacterData.BaseIntelligence;
        //_defense = trainedCharacterData.Defense;
        _defence = 0f;
        //_searchEnemyDistance = trainedCharacterData.SearchEnemyDistance;
        _attackRange = 1f;
        //_actionInterval = trainedCharacterData.ActionInterval;
        _actionInterval = trainedCharacterData.AddSpeed;
        _currentHp = 0f;
    }
    public float RePlaceTimer { get => _rePlaceTimer; set => _rePlaceTimer = value; }
    public RoleType RoleType { get => _roleType; set => _roleType = value; }
    public float Cost { get => _cost; set => _cost = value; }
    public float RePlaceInterval { get => _rePlaceInterval; set => _rePlaceInterval = value; }
    public uint ID { get => _id; set => _id = value; }
}
