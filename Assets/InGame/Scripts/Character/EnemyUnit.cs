
using UnityEngine;

public class EnemyUnit : UnitBase
{
    [HideInInspector]public Vector3 targetPosition; // 目標地点
    EnemyUnitData EnemyUnitData => (EnemyUnitData)UnitData;
    //private GameObject enemyImage;
    
    private AIRoute _route;
    private int _routeIndex = 1;        // ルートのインデックス

    protected override void Initialize()
    {
        targetPosition = _route.points[1];
        _characterImageGameObject = Instantiate(EnemyUnitData.enemyImage,this.transform);
        _characterImageGameObject.transform.localScale /= 2;
        animator = _characterImageGameObject.GetComponent<Animator>();
        OnRemovedEvent += Destroy;
        RotateForTarget(targetPosition);
        AnimatorTrigger(MoveTriggerCode);
    }
    public override void UpdateUnit(float deltaTime)
    {
        //ユニットの行動を記述する
        if (BattleTarget != null)
        {   // 交戦相手がいるとき、攻撃行動を取る
            Action(deltaTime);	
        }
        else
        {   // 交戦相手がいないとき一番近い敵を探す
            UnitBase enemy = InGameManager.Instance.FindNearestEnemy(this);
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
        RotateForTarget(targetPosition);
    }

    public void SetRoute(ref AIRoute route)
    {
        _route = route;
    }

    protected override void RotateForTarget(Vector3 vec)
    {
        if (vec.x > this.transform.position.x)
        {
            _characterImageGameObject.transform.eulerAngles = new Vector3(-54.227f, 180f, 0);
        }
        else
        {
            _characterImageGameObject.transform.eulerAngles = new Vector3(54.227f, 0f, 0);
        }
    }
}
