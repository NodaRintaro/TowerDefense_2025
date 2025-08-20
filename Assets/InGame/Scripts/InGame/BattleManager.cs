using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager _instace;
    public static BattleManager Instance => _instace;
    public GameObject enemyBase;   // Enemyの基地
    public Vector2 targetPosition;  // 目的地
    public List<UnitBase> Units;  // ユニット
    bool _isPaused = false;

    private void Awake()
    {
        if (_instace!= null && _instace != this) { Destroy(this.gameObject); }
        else { _instace = this; }
    }

    private void Update()
    {
        //ポーズ中ならば更新しない
        if (_isPaused) return;
        foreach(var unit in Units)
        {
            unit.UpdateUnit(Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        _instace = null;
    }
    
    //ユニットを追加するメソッド
    public void AddUnit(UnitBase unit)
    {
        Units.Add(unit);
    }

    // 敵のユニットを出現するメソッド
    public void PlaceEnemyUnit(GameObject unitPrefab)
    {
        Debug.Log("PlaceEnemyUnit");
        // 駒を生成する
        GameObject go = Instantiate(unitPrefab, transform);
        // プレイヤーの基地から出発
        go.transform.position = enemyBase.transform.position;
        // ユニットの目標を設定する
        EnemyUnit unit = go.GetComponent<EnemyUnit>();
        unit.SetTargetPosition(targetPosition);
    }
    
    public UnitBase FindNearestEnemy(UnitBase unit)
    {
        UnitBase nearest_enemy = null;
        float nearest_distance = float.MaxValue;

        foreach (UnitBase enemy in Units)
        {
            if (enemy.IsDead() || !unit.IsEnemy(enemy))
            {   // 死んでいる敵は無視する
                continue;
            }

            float distance = unit.Distance(enemy);
            if (distance < nearest_distance)
            {   // 一番近い敵を覚えておく
                nearest_enemy = enemy;
                nearest_distance = distance;
            }
        }
        // 一番近い敵を返す
        return nearest_enemy;
    }
}
