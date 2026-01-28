using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingEventBranchPresenter : MonoBehaviour
{
    private async UniTask EventBranchWithStaminaValueStaminaValue()
    {
        //List<TrainingEventData> branchDataList = _trainingEventDataGenerator.GenerateBranchEvent
        //    (_trainingEventStateMachine.CurrentEventType, _trainingEventStateMachine.CurrentEventData.EventID);

        //Debug.Log(branchDataList.Count);
        //switch (TrainingResultDecider.TrySuccessTrainingEvent(_trainingSaveData.CurrentStamina))
        //{
        //    case EventBranchType.TrainingFailed:
        //        foreach (var data in branchDataList)
        //        {
        //            if (data.BranchType == EventBranchType.TrainingFailed)
        //            {
        //                _trainingEventStateMachine.SetCurrentTrainingEvent(data);

        //                Debug.Log(data.ScenarioID);
        //                Debug.Log(_trainingEventStateMachine.CurrentEventType);
        //                ScenarioData scenarioData = _trainingEventDataGenerator.GenerateScenarioData
        //                    (_trainingEventStateMachine.CurrentEventType, data.ScenarioID);

        //                _trainingEventStateMachine.SetCurrentScenarioData(scenarioData);
        //                SetReadScenario(scenarioData);

        //                Debug.Log("シナリオセット完了");
        //            }
        //        }
        //        break;
        //    case EventBranchType.TrainingSuccess:
        //        foreach (var data in branchDataList)
        //        {
        //            if (data.BranchType == EventBranchType.TrainingSuccess)
        //            {
        //                _trainingEventStateMachine.SetCurrentTrainingEvent(data);

        //                Debug.Log(data.ScenarioID);
        //                Debug.Log(_trainingEventStateMachine.CurrentEventType);
        //                ScenarioData scenarioData = _trainingEventDataGenerator.GenerateScenarioData
        //                    (_trainingEventStateMachine.CurrentEventType, data.ScenarioID);

        //                _trainingEventStateMachine.SetCurrentScenarioData(scenarioData);
        //                SetReadScenario(scenarioData);
        //                Debug.Log("シナリオセット完了");
        //            }
        //        }
        //        break;
        //}

        //await _trainingEventStateMachine.ChangeState(TrainingEventStateType.ReadScenario);
    }
}
