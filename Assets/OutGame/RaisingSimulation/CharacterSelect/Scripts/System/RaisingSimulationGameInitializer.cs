using RaisingSimulationGameFlowStateMachine;
using UnityEngine;
using VContainer;

public class RaisingSimulationGameInitializer : MonoBehaviour
{
    private RaisingSimulationDataContainer _lifeTimeScope;

    private GameFlowStateMachine _gameFlowStateMachine;
    private DataLoadCompleteNotifier _loadingNotifier;

    public void Awake()
    {
        //DataLoadが済んだタイミングでゲームを動かし始める
        _loadingNotifier = _lifeTimeScope.Container.Resolve<DataLoadCompleteNotifier>();
        _loadingNotifier.OnDataLoadComplete += GameInit;
    }

    public async void GameInit()
    {
        JsonTrainingSaveDataRepository saveData = _lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();

        //セーブデータを確認してデータが残っていれば途中の画面からスタート
        await _gameFlowStateMachine.ChangeState(saveData.RepositoryData.CurrentScreenType);
    }
}
