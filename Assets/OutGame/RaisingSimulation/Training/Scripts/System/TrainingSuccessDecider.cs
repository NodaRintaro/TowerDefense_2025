using UnityEngine;
using System;

/// <summary>
/// トレーニングの成功判定クラス
/// </summary>
[Serializable]
public class TrainingSuccessDecider
{

    public EventBranchType TrySuccessTrainingEvent(uint stamina)
    {
        Debug.Log(stamina);
        uint trainingFileLine = 50;

        if (trainingFileLine <= stamina)
            return EventBranchType.TrainingSuccess;

        uint trainingSuccessPercentage = (trainingFileLine - stamina) * 2;

        if(UnityEngine.Random.Range(0, 100) > trainingSuccessPercentage)
            return EventBranchType.TrainingSuccess;

        return EventBranchType.TrainingFailed;
    }
}
