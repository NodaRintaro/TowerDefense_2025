/// <summary> Trainingのinterface </summary>
public interface IEventData
{
    public EventType EventType { get; }

    public void OnEventFire(TrainingCharacterData trainingCharacterData);
}

public enum EventType
{
    Training,
    SupportCard,
    Raid
}