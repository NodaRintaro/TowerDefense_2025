using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class TrainingEventManager
{
    [Inject]
    public TrainingEventManager() 
    { 
        
    }

    private Queue<IEventData> _eventList = new();

    /// <summary> Eventを追加 </summary>
    public void AddEvent()
    {

    }
}
