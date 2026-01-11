using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class TrainingEventPool
{
    private Queue<uint> _eventList = new();

    [Inject]
    public TrainingEventPool() { }

    public void EnqueueData(uint eventID)
    {
        _eventList.Enqueue(eventID);
    }

    public uint DequeueData()
    {
        return _eventList.Dequeue();
    }

    public bool IsEventQueueEmpty()
    {
        if(_eventList.Count > 0) 
            return true;

        return false;
    }
}
