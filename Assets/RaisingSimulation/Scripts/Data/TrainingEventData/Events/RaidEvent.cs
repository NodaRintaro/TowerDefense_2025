using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidEvent : IEventData
{
    [SerializeField] EventType _eventType;

    public EventType EventType => _eventType;

    public void OnEventFire(TrainingCharacterData trainingCharacterData)
    {

    }
}
