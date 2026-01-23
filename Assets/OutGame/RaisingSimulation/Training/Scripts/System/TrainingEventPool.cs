using System.Collections.Generic;
using VContainer;
using UnityEngine;

/// <summary> 1ターンで行われるEventの登録先 </summary>
public class TrainingEventPool
{
    private Dictionary<TrainingEventType, Queue<uint>> _trainingEventPool = new Dictionary<TrainingEventType, Queue<uint>>();

    [Inject]
    public TrainingEventPool()
    {
        _trainingEventPool.Add(TrainingEventType.TrainingEvent, new Queue<uint>());
        _trainingEventPool.Add(TrainingEventType.CharacterUniqueEvent, new Queue<uint>());
        _trainingEventPool.Add(TrainingEventType.SupportCardEvent, new Queue<uint>());
    }

    public void EnqueueData(TrainingEventType trainingEventType, uint eventID)
    {
        if(eventID != 0)
        {
            _trainingEventPool[trainingEventType].Enqueue(eventID);

            Debug.Log(trainingEventType + "の" + eventID + "データが入りました");
        }
    }

    public uint DequeueData(TrainingEventType trainingEventType)
    {
        if(IsEventQueueEmpty(trainingEventType))
        {
            return 0;
        }

        return _trainingEventPool[trainingEventType].Dequeue();
    }

    public bool IsEventQueueEmpty(TrainingEventType trainingEventType)
    {
        if (_trainingEventPool[trainingEventType].Count > 0) 
            return false;

        return true;
    }
}
