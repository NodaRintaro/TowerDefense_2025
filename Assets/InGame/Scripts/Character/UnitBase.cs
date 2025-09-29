using System;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [SerializeField] private HPBar _hpBar;   // HPバー
    //所属グループ
    private GroupType _group = GroupType.Enemy;
    private float _maxHp;            // 最大HP
    private float _attack;           // 攻撃力
    private float _defense;          // 防御力
    public float searchEnemyDistance;      // 索敵範囲
    public float actionInterval;           // 行動間隔
    protected bool _isDead = false;        // 死亡フラグ
    protected float ActionWait;            // 次の行動までの時間
    private float _currentHp;                // 現在のＨＰ
    public int ID;                         // ユニットID
    protected UnitBase BattleTarget;       // 交戦相手
    protected UnitData UnitData;           // ユニットデータ
    
    public event Action OnRemovedEvent;   //ユニット削除時イベント
    public event Action<float> OnHealthChangedEvent;   //ユニット選択時イベント

    public bool IsDead
    {
        get { return _isDead; }
        private set
        {
            _isDead = value; 
        }
    }
    protected float CurrentHp
    {
        get { return _currentHp; }
        set
        {
            _currentHp = value;
            OnHealthChangedEvent?.Invoke(value);
        }
    }
    public void Init()
    {
        // ユニットリストに自分を追加する
        InGameManager.Instance.AddUnit(this);
        Initialize();
        // HPバーを初期化
        _hpBar.Init(_maxHp);
        OnHealthChangedEvent += _hpBar.UpdateHp;
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
        return _group != targetUnit._group;
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
        float damage = Mathf.Max(_attack - target._defense, 0);
        // 攻撃相手はダメージを受ける
        target.GetDamage(damage);
    }
    
    // ダメージを受ける
    protected void GetDamage(float damage)
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

    public void SetUnitData(UnitData unitData, GroupType group)
    {
        UnitData = unitData;
        _currentHp = unitData.hp;
        _maxHp = unitData.hp;
        _attack = unitData.atk;
        _defense = unitData.defense;
        _group = group;
        searchEnemyDistance = unitData.range;
        actionInterval = unitData.attackSpeed;
    }

    public void SetUnitData(EnemyData enemyData)
    {
        _currentHp = enemyData.hp;
        _maxHp = enemyData.hp;
        _attack = enemyData.attack;
        _defense = enemyData.defence;
        searchEnemyDistance = enemyData.range;
        actionInterval = enemyData.speed;
        _group = GroupType.Enemy;
    }
    public enum GroupType
    {
        Player,
        Enemy,
    }
}