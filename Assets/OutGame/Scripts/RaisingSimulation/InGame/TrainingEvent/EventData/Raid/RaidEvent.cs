using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidEvent : EventBase
{
    [SerializeField] EventType _eventType;

    public EventType EventType => _eventType;

    public override void OnEventAction()
    {
        
    }

    public override void OnFinishAction()
    {
        
    }
}
