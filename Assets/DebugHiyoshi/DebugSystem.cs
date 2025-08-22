using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DebugSystem : MonoBehaviour
{
    BattleManager battleManager;
    [SerializeField] private GameObject enemyPrefab;
    void Start()
    {
        battleManager = BattleManager.Instance;
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            battleManager.PlaceEnemyUnit(enemyPrefab);   
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            battleManager.ChangeTimeSpeed(2);   
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            battleManager.ChangeTimeSpeed(1);   
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            battleManager.ChangeTimeSpeed(0.5f);   
        }
    }
}
