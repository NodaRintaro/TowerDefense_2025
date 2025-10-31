using UnityEngine;

/// <summary>
/// キャラクターの特定のステータスを上昇させるクラス
/// </summary>
[System.Serializable]
public class EventStatusBuff
{
    [SerializeField, Header("トレーニングの内容")]
    private BuffType _trainingBuffType;

    [SerializeField, Header("トレーニングの基礎強化値")]
    private int _baseBuff; 

    public int StatusBuffPoints => _baseBuff;

    /// <summary> トレーニングによるキャラのステータスの向上 </summary>
    public void BuffStatus(TrainingCharacterData trainingCharacter, float bonusBuffPercent)
    {
        uint bonusBuff = (uint)Mathf.Floor(bonusBuffPercent * _baseBuff);
        uint totalBuff = (uint)_baseBuff + bonusBuff;

        switch (_trainingBuffType)
        {
            case BuffType.Physical:
                trainingCharacter.AddCurrentPhysical(totalBuff); 
                break;
            case BuffType.Power:
                trainingCharacter.AddCurrentPower(totalBuff);
                break;
            case BuffType.Intelligence:
                trainingCharacter.AddCurrentIntelligence(totalBuff);
                break;
            case BuffType.Speed:
                trainingCharacter.AddCurrentSpeed(totalBuff);
                break;
            case BuffType.TakeBreak:
                trainingCharacter.TakeBreak(totalBuff);
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
