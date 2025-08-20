using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : UnitBase
{
    public Vector3 targetPosition; // 目標地点
    public float moveSpeed;        // 移動速度

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public override void UpdateUnit(float deltaTime)
    {
        //ユニットの行動を記述する
        if (battleTarget != null)
        {   // 交戦相手がいるとき、攻撃行動を取る
            AttackAction(deltaTime);	
        }
        else
        {   // 交戦相手がいないとき一番近い敵を探す
            UnitBase enemy = BattleManager.Instance.FindNearestEnemy(this);
            if(enemy != null && Distance(enemy) <= searchEnemyDistance)
            {   // 一番近い敵が索敵範囲内なら交戦に入る
                battleTarget = enemy;
            }
            else
            {   // いなかったら目的地に向かって移動する
                MoveAction(deltaTime);	
            }
        }
    }
    void MoveAction(float deltaTime)
    {
        // 目的地に向かって移動する
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * deltaTime);
    }
    
}
