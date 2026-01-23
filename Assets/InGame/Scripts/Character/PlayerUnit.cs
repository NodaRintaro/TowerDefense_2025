using UnityEngine;

public class PlayerUnit : UnitBase
{
    [SerializeField] private SpriteRenderer _characterSprite;
    public bool _isBack = false; //キャラクターをクリックしたときにデッキから戻る用のbool値
    protected override void Initialize()
    {
        GetComponent<BoxCollider>().enabled = true;
        Debug.Log($"Range{UnitData.AttackRange}\nSpeed{UnitData.ActionInterval}");
    }
    
    public void SetImage(Sprite sprite)
    {
        _characterSprite.sprite = sprite;
    }
    
    // ユニットの状態を更新する
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
            if(enemy != null && Distance(enemy) <= UnitData.AttackRange)
            {   // 一番近い敵が索敵範囲内なら交戦に入る
                BattleTarget = enemy;
            }
        }
    }
}
