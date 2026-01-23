using System;
using UnityEngine;
using DG.Tweening;
public class UnitBase : MonoBehaviour
{
    [SerializeField] private HPBar _hpBar;   // HPバー
    [SerializeField] protected GameObject _characterImageGameObject;
    private UnitBase _battleTarget;
    public UnitData UnitData;           // ユニットデータ
    private float _durationSeconds = 0.2f;
    
    protected static readonly int AttackTriggerCode = Animator.StringToHash("AttackTrigger");
    protected Animator animator;
    public PlayerUnitData PlayerData => UnitData as PlayerUnitData;   // プレイヤーユニットデータ
    public EnemyUnitData EnemyData => UnitData as EnemyUnitData;   // 敵ユニットデータ
    
    public event Action OnRemovedEvent;   //ユニット削除時イベント
    public event Action<float> OnHealthChangedEvent;

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
    protected UnitBase BattleTarget 
    { 
        get => _battleTarget;
        set
        {
            _battleTarget = value;
            if(value == null) RotateForTarget(Vector3.zero);
            else RotateForTarget(value.transform.position);
        } 
    }       // 交戦相手

    public bool IsFullHp => CurrentHp >= UnitData.MaxHp;
        
    
    #endregion
    public void Init()
    {
        // ユニットリストに自分を追加する
        InGameManager.Instance.AddUnit(this);
        Initialize();
        // HPバーを初期化
        _hpBar.Init(UnitData.Attack);
        IsDead = false;
        OnHealthChangedEvent += _hpBar.UpdateHp;
    }

    protected virtual void Initialize() { }
    
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
    protected void Action(float deltaTime)
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
        AnimatorTrigger(AttackTriggerCode);
        // 自分の攻撃力から相手の防御力を引いたものをダメージとする（０未満にはならない）
        float damage = Mathf.Max(UnitData.Attack - target.UnitData.Defence, 0);
        if (UnitData.JobType == JobType.Healer)
        {
            if(IsEnemy(target))
            {   // 敵ユニットには回復しない
                return;
            }
            target.GetHeal(UnitData.MagicPower);
        }
        else
        {
            if(!IsEnemy(target))
            {   // 敵じゃないユニットには攻撃しない
                return;
            }
            target.GetDamage(damage);
            // 攻撃相手はダメージを受ける
        }
        AnimatorTrigger(AttackTriggerCode);
    }
    
    // ダメージを受ける
    protected void GetDamage(float damage)
    {
        // HPが０未満にならないようにダメージを受ける
        Debug.Log($"<color=red>{gameObject.name}Current hp{CurrentHp} - damage{damage}</color>");
        CurrentHp = Mathf.Max(CurrentHp - damage, 0);
        Debug.Log($"Current hp{CurrentHp}");
        if(CurrentHp == 0)
        {
            IsDead = true;
        }
        else
        {
            SpriteRenderer sprite = _characterImageGameObject.GetComponent<SpriteRenderer>();
            var tween = sprite.DOColor(new Color(0.7f, 0f, 0f, 1f), _durationSeconds).SetEase(Ease.INTERNAL_Zero);
            tween.onComplete = () => sprite.color = new Color(1,1,1,1);
        }
    }

    public void GetHeal(float  heal)
    {
        Debug.Log($"<color=green>{gameObject.name}Current hp{CurrentHp} + heal{heal}</color>");
        CurrentHp = Mathf.Min(CurrentHp + heal, UnitData.MaxHp);
        Debug.Log($"Current hp{CurrentHp}");
    }
    public float Distance(UnitBase targetUnit)
    {
        return Vector3.Distance(transform.position, targetUnit.transform.position);
    }

    protected void AnimatorTrigger(int trigger)
    {
        if (animator == null)
            return;
        animator.SetTrigger("AttackTrigger");
    }

    protected virtual void RotateForTarget(Vector3 vec)//Targetの方向にユニットを向ける
    { }
}