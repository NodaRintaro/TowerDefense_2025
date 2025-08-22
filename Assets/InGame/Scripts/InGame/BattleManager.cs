using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager _instace;
    public static BattleManager Instance => _instace;
    public AIRoutes aiRoutes;                      // Enemyの基地, Enemyの出撃地点はIndex０、それ以降は敵が通るルート
    public List<UnitBase> unitList;                   // ユニット
    private bool _isPaused = false;
    private float _timeSpeed = 1;                   //ゲーム内の時間の速さ

    #region Unity Functions
    private void Awake()
    {
        if (_instace!= null && _instace != this) { Destroy(this.gameObject); }
        else { _instace = this; }
    }

    private void Update()
    {
        //ポーズ中ならば更新しない
        if (_isPaused) return;
        float timeSpeed = _timeSpeed * Time.deltaTime;
        foreach(var unit in unitList)
        {
            unit.UpdateUnit(timeSpeed);
        }
    }

    private void OnDestroy()
    {
        _instace = null;
    }
    #endregion
    
    //ユニットを追加するメソッド
    public void AddUnit(UnitBase unit)
    {
        unitList.Add(unit);
    }

    // 敵のユニットを出現するメソッド
    public void PlaceEnemyUnit(GameObject unitPrefab)
    {
        Debug.Log("InstanciateEnemyUnit");
        // 駒を生成する
        GameObject go = Instantiate(unitPrefab, aiRoutes.Points[0].position, Quaternion.identity);
        // プレイヤーの基地から出発
        go.transform.position = aiRoutes.Points[0].position;
        // ユニットの目標を設定する
        EnemyUnit unit = go.GetComponent<EnemyUnit>();
        unit.SetTargetPosition(aiRoutes.Points[1].position);
    }
    
    //最寄りの敵対ユニットを返す
    public UnitBase FindNearestEnemy(UnitBase unit)
    {
        UnitBase nearest_enemy = null;
        float nearest_distance = float.MaxValue;

        foreach (UnitBase enemy in unitList)
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

    public Vector3 GetTargetPosition(UnitBase unit, int index)
    {
        if (index >= aiRoutes.Points.Count)
        {
            unitList.Remove(unit);
            GetEnemyOnGoal();
            return new Vector3(0,0,0);
        }
        return aiRoutes.Points[index].position;
    }
    private void GetEnemyOnGoal()
    {
        Debug.Log("敵が防衛ラインを突破！！");
    }
    public void ChangeTimeSpeed(float timeSpeed)
    {
        _timeSpeed = timeSpeed;
    }
}
