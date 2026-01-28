using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary> 1ターンで行われるEventの登録先 </summary>
public class TrainingEventPool
{
    private Dictionary<TrainingEventType, Queue<uint>> _trainingEventPool = new Dictionary<TrainingEventType, Queue<uint>>();

    private TrainingEventDataGenerator _trainingEventDataGenerator = null;

    [Inject]
    public TrainingEventPool(TrainingEventDataGenerator trainingEventDataGenerator)
    {
        _trainingEventDataGenerator = trainingEventDataGenerator;

        _trainingEventPool.Add(TrainingEventType.TrainingEvent, new Queue<uint>());
        _trainingEventPool.Add(TrainingEventType.CharacterUniqueEvent, new Queue<uint>());
        _trainingEventPool.Add(TrainingEventType.SupportCardEvent, new Queue<uint>());
        _trainingEventDataGenerator = trainingEventDataGenerator;
    }

    public bool TryGetNextTrainingEvent(out TrainingEventData trainingEventData, ref TrainingEventType currentEventType)
    {
        trainingEventData = null;
        switch (currentEventType)
        {
            case TrainingEventType.TrainingEvent:
                if (IsEventQueueEmpty(TrainingEventType.TrainingEvent))
                {
                    Debug.Log(currentEventType + "のイベントは空っぽ");
                    currentEventType = TrainingEventType.CharacterUniqueEvent;
                    TryGetNextTrainingEvent(out trainingEventData, ref currentEventType);
                    break;
                }
                else
                {
                    CurrentTrainingDataGenerate(currentEventType);
                    return true;
                }
            case TrainingEventType.CharacterUniqueEvent:
                if (IsEventQueueEmpty(TrainingEventType.CharacterUniqueEvent))
                {
                    Debug.Log(currentEventType + "のイベントは空っぽ");
                    currentEventType = TrainingEventType.SupportCardEvent;
                    TryGetNextTrainingEvent(out trainingEventData, ref currentEventType);
                    break;
                }
                else
                {
                    trainingEventData = CurrentTrainingDataGenerate(currentEventType);
                    return true;
                }
            case TrainingEventType.SupportCardEvent:
                if (IsEventQueueEmpty(TrainingEventType.SupportCardEvent))
                {
                    Debug.Log(currentEventType + "のイベントは空っぽ");
                    currentEventType = TrainingEventType.None;
                    return false;
                }
                else
                {
                    trainingEventData = CurrentTrainingDataGenerate(currentEventType); 
                    return true;
                }
        }
        return false;
    }

    private TrainingEventData CurrentTrainingDataGenerate(TrainingEventType currentEventType)
    {
        uint dequeueData = DequeueData(currentEventType);

        return _trainingEventDataGenerator.GenerateTrainingEvent(currentEventType, dequeueData);
    }

    public void EnqueueData(TrainingEventType trainingEventType, uint eventID)
    {
        if(eventID != 0)
        {
            _trainingEventPool[trainingEventType].Enqueue(eventID);

            Debug.Log(trainingEventType + "の" + eventID + "データが入りました");
        }
    }

    private uint DequeueData(TrainingEventType trainingEventType)
    {
        return _trainingEventPool[trainingEventType].Dequeue();
    }

    private bool IsEventQueueEmpty(TrainingEventType trainingEventType)
    {
        if (_trainingEventPool[trainingEventType].Count > 0) 
            return false;

        return true;
    }
}
