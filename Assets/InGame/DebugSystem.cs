using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSystem : MonoBehaviour
{
    BattleManager battleManager;
    void Start()
    {
        battleManager = BattleManager.Instance;
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            battleManager.PlaceEnemyUnit(battleManager.enemyBase);   
        }
    }
}
