using System;
using UnityEngine;
using Unity.VisualScripting;

[Serializable]
public abstract class EventBase
{
    [SerializeField] private IEventAction[] eventActions;

    public abstract void OnEventAction();

    public abstract void OnFinishAction();
}