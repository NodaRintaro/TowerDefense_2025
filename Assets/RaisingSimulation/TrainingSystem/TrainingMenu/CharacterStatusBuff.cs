using UnityEngine;

/// <summary>
/// キャラクターの特定のステータスを上昇させるクラス
/// </summary>
[System.Serializable]
public class CharacterStatusBuff
{
    [SerializeField, Header("トレーニングの内容")]
    private BuffType _trainingBuffType;

    [SerializeField, Header("トレーニングの基礎強化値")]
    private int _baseBuffPoints; 

    public int StatusBuffPoints => _baseBuffPoints;

    /// <summary> トレーニングによるキャラのステータスの向上 </summary>
    public void BuffStatus(TrainingCharacterData trainingCharacter, uint bonusEnhancePoints)
    {
        uint totalEnhancePoints = (uint)_baseBuffPoints + bonusEnhancePoints;

        switch (_trainingBuffType)
        {
            case BuffType.Physical:
                trainingCharacter.AddCurrentPhysical(totalEnhancePoints); 
                break;
            case BuffType.Power:
                trainingCharacter.AddCurrentPower(totalEnhancePoints);
                break;
            case BuffType.Intelligence:
                trainingCharacter.AddCurrentIntelligence(totalEnhancePoints);
                break;
            case BuffType.Speed:
                trainingCharacter.AddCurrentSpeed(totalEnhancePoints);
                break;
            case BuffType.TakeBreak:
                trainingCharacter.TakeBreak(totalEnhancePoints);
                break;
        }
    }
}

/// <summary>
/// トレーニングの内容を決めるenum
/// </summary>
public enum BuffType{
    Physical,
    Power,
    Intelligence,
    Speed,
    TakeBreak
}
