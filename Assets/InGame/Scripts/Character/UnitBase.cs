using System;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    //所属グループ
    [SerializeField] private GroupType group;
    [SerializeField] private int maxHp;            // 最大HP
    [SerializeField] private int attack;           // 攻撃力
    [SerializeField] private int defense;          // 防御力
    public float searchEnemyDistance;      // 索敵範囲
    public float actionInterval;           // 行動間隔
    protected bool _isDead = false;        // 死亡フラグ
    protected float ActionWait;            // 次の行動までの時間
    protected int CurrentHp;               // 現在のＨＰ
    public int ID;                         // ユニットID
    protected UnitBase BattleTarget;       // 交戦相手
    
    public event Action OnRemovedEvent;   //ユニット削除時イベント

    public bool IsDead
    {
        get { return _isDead; }
        private set
        {
            _isDead = value; 
        }
    }
    public void Init()
    {
        // ユニットリストに自分を追加する
        InGameManager.Instance.AddUnit(this);
        Initialize();
    }

    public virtual void Initialize() { }
    
    // ユニットの状態を更新する
    public virtual void UpdateUnit(float deltaTime) { }

    public void Remove()
    {
        OnRemovedEvent?.Invoke();
    }
    //敵陣営か判定
    public bool IsEnemy(UnitBase targetUnit)
    {
        return group != targetUnit.group;
    }

    // 攻撃行動ををするメソッド
    protected void AttackAction(float deltaTime)
    {
        // 次の行動間隔まで待つ
        ActionWait -= deltaTime;
        if (ActionWait > 0) return;

        // 次の行動をするまでの待ち時間をセット
        ActionWait += actionInterval;

        // 攻撃する
        Attack(BattleTarget);
        if (BattleTarget.IsDead)
        {
            // 相手を倒したらターゲットから外す
            BattleTarget = null;
        }
    }

    // targetのユニットに対して攻撃する
    protected void Attack(UnitBase target)
    {
        if(!IsEnemy(target))
        {   // 敵じゃないユニットには攻撃しない
            return;
        }
        // 自分の攻撃力から相手の防御力を引いたものをダメージとする（０未満にはならない）
        int damage = Mathf.Max(attack - target.defense, 0);
        // 攻撃相手はダメージを受ける
        target.GetDamage(damage);
    }
    
    // ダメージを受ける
    protected void GetDamage(int damage)
    {
        // HPが０未満にならないようにダメージを受ける
        CurrentHp = Mathf.Max(CurrentHp - damage, 0);
        if(CurrentHp == 0)
        {
            IsDead = true;
        }
    }
    public float Distance(UnitBase targetUnit)
    {
        return Vector3.Distance(transform.position, targetUnit.transform.position);
    }
    public enum GroupType
    {
        Player,
        Enemy,
    }
}