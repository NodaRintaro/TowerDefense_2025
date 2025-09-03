/// <summary> Trainingのinterface </summary>
public interface ITrainingMenu
{
    public TrainingType TrainingType 
    {
        get;
    }

    public void TrainingEvent(TrainingCharacterData trainingCharacterData);
}