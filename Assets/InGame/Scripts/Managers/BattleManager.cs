using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager _instace;
    public static BattleManager Instance => _instace;
    public AIRoutes aiRoutes;                         // Enemyの出撃地点はIndex０、それ以降は敵が通るルート
    public TrainedCharacterRegistry trainedCharacters;// テスト用！育成済みキャラクターデータ
    [HideInInspector]
    public List<UnitBase> unitList;                   // ユニットのリスト
    private bool _isPaused = false;                   //ポーズ中かどうか
    private float _timeSpeed = 1;                     //ゲーム内の時間の速さ

    #region Unity Functions
    private void Awake()
    {
        if (_instace != null && _instace != this) { Destroy(this.gameObject); }
        else { _instace = this; }
    }

    private void Update()
    {
        //ポーズ中ならば更新しない
        if (_isPaused) return;
        float timeSpeed = _timeSpeed * Time.deltaTime;
        
        for(int i = 0; i < unitList.Count; i++)
        {
            UnitBase unit = unitList[i];
            if (unit.IsDead)
            {
                RemoveUnit(unit);
            }
            else
            {
                unit.UpdateUnit(timeSpeed);   
            }
        }
    }

    private void OnDestroy()
    {
        _instace = null;
    }
    #endregion

    #region  Battle Functions
    //ユニットを追加するメソッド
    public void AddUnit(UnitBase unit)
    {
        unitList.Add(unit);
    }
    //ユニットを削除するメソッド
    public void RemoveUnit(UnitBase unit)
    {
        unitList.Remove(unit);
        Destroy(unit.gameObject);
    }

    // 敵のユニットを出現するメソッド
    public void PlaceEnemyUnit(GameObject unitPrefab)
    {
        // 駒を生成する
        GameObject go = Instantiate(unitPrefab, aiRoutes.Points[0], Quaternion.identity);
        // プレイヤーの基地から出発
        go.transform.position = aiRoutes.Points[0];
        // ユニットの目標を設定する
        EnemyUnit unit = go.GetComponent<EnemyUnit>();
        unit.SetTargetPosition(aiRoutes.Points[1]);
        unit.Init();
    }
    
    //最寄りの敵対ユニットを返す
    public UnitBase FindNearestEnemy(UnitBase unit)
    {
        UnitBase nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (UnitBase enemy in unitList)
        {
            if (enemy.IsDead || !unit.IsEnemy(enemy))
            {   // 死んでいる敵は無視する
                continue;
            }

            float distance = unit.Distance(enemy);
            if (distance < nearestDistance)
            {   // 一番近い敵を覚えておく
                nearestEnemy = enemy;
                nearestDistance = distance;
            }
        }
        // 一番近い敵を返す
        return nearestEnemy;
    }
    
    public Vector3 GetTargetPosition(UnitBase unit, int index)
    {
        if (index >= aiRoutes.Points.Count)
        {
            RemoveUnit(unit);
            GetEnemyOnGoal();
            return new Vector3(0,0,0);
        }
        return aiRoutes.Points[index];
    }
    public void GetEnemyOnGoal()
    {
        Debug.Log("敵が防衛ラインを突破！！");
    }
    public void ChangeTimeSpeed(float timeSpeed)
    {
        _timeSpeed = timeSpeed;
    }
    #endregion
}
