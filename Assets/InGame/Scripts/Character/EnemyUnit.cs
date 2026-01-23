
using UnityEngine;

public class EnemyUnit : UnitBase
{
    [HideInInspector]public Vector3 targetPosition; // 目標地点
    EnemyUnitData EnemyUnitData => (EnemyUnitData)UnitData;
    
    private AIRoute _route;
    //public float moveSpeed;             // 移動速度
    private int _routeIndex = 1;        // ルートのインデックス

    protected override void Initialize()
    {
        targetPosition = _route.points[1];
        GameObject obj = Instantiate(EnemyUnitData.enemyImage,this.transform);
        obj.transform.localScale /= 2;
        animator = obj.GetComponent<Animator>();
        OnRemovedEvent += Destroy;
    }
    public override void UpdateUnit(float deltaTime)
    {
        Debug.Log(BattleTarget == null ? "Null" : "Not Null");
        //ユニットの行動を記述する
        if (BattleTarget != null)
        {   // 交戦相手がいるとき、攻撃行動を取る
            Action(deltaTime);	
        }
        else
        {   // 交戦相手がいないとき一番近い敵を探す
            UnitBase enemy = InGameManager.Instance.FindNearestTarget(this);
            if(enemy != null && Distance(enemy) <= EnemyUnitData.AttackRange)
            {   // 一番近い敵が索敵範囲内なら交戦に入る
                BattleTarget = enemy;
            }
            else
            {   // いなかったら目的地に向かって移動する
                MoveAction(deltaTime);	
            }
        }
    }

    private void Destroy()
    {
        // 敵の死亡処理
        Destroy(gameObject);
    }
    //移動処理
    private void MoveAction(float deltaTime)
    {
        Debug.Log("Moving");
        // 目的地に向かって移動する
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, EnemyUnitData.MoveSpeed * deltaTime);
        if (transform.position == targetPosition)
        {
            ArriveTargetPosition();
        }
    }

    /// <summary>
    /// 目標としている地点に到着したときの処理
    /// </summary>
    private void ArriveTargetPosition()
    {
        if (_route.points.Length <= _routeIndex+ 1)
        {
            InGameManager.Instance.EnemyArriveGoal(this);
            return;
        }
        _routeIndex++;
        targetPosition = _route.points[_routeIndex];
    }

    public void SetRoute(ref AIRoute route)
    {
        _route = route;
    }
}
