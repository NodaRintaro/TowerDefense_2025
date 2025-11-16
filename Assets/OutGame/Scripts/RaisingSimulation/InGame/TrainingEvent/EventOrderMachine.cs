using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class EventOrderMachine
{
    private Queue<EventBase> _eventList = new();

    [Inject]
    public EventOrderMachine()
    { 
        
    }

    /// <summary> Eventを追加 </summary>
    public void AddEvent()
    {

    }
}
