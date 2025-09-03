using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSystem : MonoBehaviour
{
    private BattleManager _battleManager;
    [SerializeField] private GameObject enemyPrefab;
    void Start()
    {
        _battleManager = BattleManager.Instance;
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _battleManager.PlaceEnemyUnit(enemyPrefab);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _battleManager.ChangeTimeSpeed(2);   
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _battleManager.ChangeTimeSpeed(1);   
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _battleManager.ChangeTimeSpeed(0.5f);   
        }
    }
}
