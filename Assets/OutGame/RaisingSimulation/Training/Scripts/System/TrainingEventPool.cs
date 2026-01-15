using System.Collections.Generic;
using VContainer;

/// <summary> 1ターンで行われるEventの登録先 </summary>
public class TrainingEventPool
{
    private Queue<TrainingEventData> _eventList = new();

    [Inject]
    public TrainingEventPool() { }

    public void EnqueueData(TrainingEventData eventID)
    {
        _eventList.Enqueue(eventID);
    }

    public TrainingEventData DequeueData()
    {
        if(IsEventQueueEmpty())
        {
            return null;
        }
        return _eventList.Dequeue();
    }

    public bool IsEventQueueEmpty()
    {
        if(_eventList.Count > 0) 
            return false;

        return true;
    }
}
