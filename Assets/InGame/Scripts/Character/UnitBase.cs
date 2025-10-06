using System;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [SerializeField] private HPBar _hpBar;   // HPバー
    protected UnitBase BattleTarget;       // 交戦相手
    public UnitData UnitData;           // ユニットデータ
    
    public event Action OnRemovedEvent;   //ユニット削除時イベント
    public event Action<float> OnHealthChangedEvent;   //ユニット選択時イベント

    #region Property
    public bool IsDead
    {
        get { return UnitData.IsDead; }
        private set
        {
            UnitData.IsDead = value; 
        }
    }
    protected float CurrentHp
    {
        get { return UnitData.CurrentHp; }
        set
        {
            UnitData.CurrentHp = value;
            OnHealthChangedEvent?.Invoke(value);
        }
    }
    #endregion
    public void Init()
    {
        // ユニットリストに自分を追加する
        InGameManager.Instance.AddUnit(this);
        Initialize();
        // HPバーを初期化
        _hpBar.Init(UnitData.Attack);
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
        return UnitData.Group != targetUnit.UnitData.Group;
    }

    // 攻撃行動ををするメソッド
    protected void AttackAction(float deltaTime)
    {
        // 次の行動間隔まで待つ
        UnitData.ActionTimer -= deltaTime;
        if (UnitData.ActionTimer > 0) return;

        // 次の行動をするまでの待ち時間をセット
        UnitData.ActionTimer += UnitData.ActionInterval;

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
        float damage = Mathf.Max(UnitData.Attack - target.UnitData.Defence, 0);
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

    public void Heal(float heal)
    {
        CurrentHp = Mathf.Min(CurrentHp + heal, UnitData.MaxHp);
    }
    public float Distance(UnitBase targetUnit)
    {
        return Vector3.Distance(transform.position, targetUnit.transform.position);
    }
}