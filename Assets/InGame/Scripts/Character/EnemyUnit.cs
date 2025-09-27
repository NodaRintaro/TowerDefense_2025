using UnityEngine;

public class EnemyUnit : UnitBase
{
    [HideInInspector]public Vector3 targetPosition; // 目標地点
    private AIRoutes _routes; 　　　//ルート管理クラス
    public float moveSpeed;        // 移動速度
    private int _routeIndex = 1;   // ルートのインデックス

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public override void UpdateUnit(float deltaTime)
    {
        //ユニットの行動を記述する
        if (BattleTarget != null)
        {   // 交戦相手がいるとき、攻撃行動を取る
            AttackAction(deltaTime);	
        }
        else
        {   // 交戦相手がいないとき一番近い敵を探す
            UnitBase enemy = InGameManager.Instance.FindNearestEnemy(this);
            if(enemy != null && Distance(enemy) <= searchEnemyDistance)
            {   // 一番近い敵が索敵範囲内なら交戦に入る
                BattleTarget = enemy;
            }
            else
            {   // いなかったら目的地に向かって移動する
                MoveAction(deltaTime);	
            }
        }
    }
    //移動処理
    private void MoveAction(float deltaTime)
    {
        // 目的地に向かって移動する
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * deltaTime);
        if (transform.position == targetPosition)
        {
            GetTargetPosition(this);
        }
    }

    private void GetTargetPosition(UnitBase unit)
    {
        _routeIndex++;
        targetPosition = InGameManager.Instance.GetTargetRoutePosition(unit, _routeIndex);
    }
}
