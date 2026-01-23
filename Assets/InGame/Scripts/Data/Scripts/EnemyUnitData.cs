using UnityEngine;

public class EnemyUnitData : UnitData
{
    public GameObject enemyImage;
    private float _moveSpeed;
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    public EnemyUnitData(EnemyData enemyData)
    {
        _moveSpeed = enemyData.moveSpeed;
        Name = enemyData.enemyName;
        MaxHp = enemyData.hp;
        Attack = enemyData.attack;
        Defence = enemyData.defence;
        ActionInterval = enemyData.attackRate;
        AttackRange = enemyData.range;
        enemyImage = enemyData.enemyImage;
        Group = GroupType.Enemy;
    }
}
