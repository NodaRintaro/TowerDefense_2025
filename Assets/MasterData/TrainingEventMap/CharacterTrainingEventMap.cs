using UnityEngine;

public class CharacterTrainingEventMap
{
    [SerializeField] OneDayEvent[] _trainingEventMap;

    public OneDayEvent[] trainingEventMap => _trainingEventMap;
}

public struct OneDayEvent
{
    public bool Raid;

    public TrainingEventData UniqueEventFirst;

    public TrainingEventData UniqueEventSecond;

    public TrainingEventData UniqueEventLast;
}
