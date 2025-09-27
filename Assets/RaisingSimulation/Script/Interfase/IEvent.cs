/// <summary> Trainingのinterface </summary>
public interface IEvent
{
    public TrainingType TrainingType { get; }

    public void OnTrainingEvent(TrainingCharacterData trainingCharacterData);
}