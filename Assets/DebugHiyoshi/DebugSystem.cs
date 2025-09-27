using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSystem : MonoBehaviour
{
    private InGameManager _inGameManager;
    void Start()
    {
        _inGameManager = InGameManager.Instance;
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _inGameManager.ChangeTimeSpeed(2);   
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _inGameManager.ChangeTimeSpeed(1);   
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _inGameManager.ChangeTimeSpeed(0.5f);   
        }
    }
}
