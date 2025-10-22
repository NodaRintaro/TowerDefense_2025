public class UnitData
{
    protected GroupType _group;               // ユニットが属しているグループ
    protected string _name;                   // ユニット名
    protected float _maxHp;                   // 最大HP
    protected float _attack;                  // 攻撃力
    protected float _magicPower;              // 魔法力
    protected float _defence;                 // 防御力
    protected float _attackRange;             // 索敵範囲
    protected float _actionInterval;          // 行動間隔
    protected float _actionTimer;             // 行動時間計測用タイマー
    protected float _currentHp;               // 現在のＨＰ
    protected bool _isDead = false;           // 死亡フラグ
    protected UnitData() { }

    public UnitData(EnemyData enemyData)
    {
        _name = enemyData.enemyName;
        _maxHp = enemyData.hp;
        
    }

    #region Properties
    public GroupType Group { get => _group; set => _group = value; }
    public string Name { get => _name; set => _name = value; }
    public bool IsDead { get => _isDead; set => _isDead = value; }
    public float MaxHp { get => _maxHp; set => _maxHp = value; }
    public float Attack { get => _attack; set => _attack = value; }
    public float Defence { get => _defence; set => _defence = value; }
    public float AttackRange { get => _attackRange; set => _attackRange = value; }
    public float ActionInterval { get => _actionInterval; set => _actionInterval = value; }
    public float CurrentHp { get => _currentHp; set => _currentHp = value; }
    public float ActionTimer { get => _actionTimer; set => _actionTimer = value; }
    #endregion
}
public enum GroupType
{
    Player,
    Enemy,
}