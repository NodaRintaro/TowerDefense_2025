using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class TrainingEventSelecter
{
    [Inject]
    public TrainingEventSelecter() { }

    private Queue<IEventData> _eventList = new();

    /// <summary> Eventを追加 </summary>
    public void AddEvent()
    {

    }
}
