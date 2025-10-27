/// <summary> Trainingのinterface </summary>
public interface IEventData
{
    public TrainingType TrainingType { get; }

    public void OnTrainingEvent(TrainingCharacterData trainingCharacterData);
}