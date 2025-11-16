public class EnemyUnitData : UnitData
{
    private float _moveSpeed;
    //private AIRoute _aiRoute;
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    // public AIRoute AiRoute
    // {
    //     get { return _aiRoute; }
    //     set { _aiRoute = value; }
    // }

    public EnemyUnitData(EnemyData enemyData)
    {
        _moveSpeed = enemyData.moveSpeed;
        Name = enemyData.enemyName;
        MaxHp = enemyData.hp;
        Attack = enemyData.attack;
        Defence = enemyData.defence;
        ActionInterval = enemyData.attackRate;
        AttackRange = enemyData.range;
        Group = GroupType.Enemy;
    }
}
