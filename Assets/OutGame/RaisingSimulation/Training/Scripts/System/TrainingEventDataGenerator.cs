using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class TrainingEventDataGenerator
{
    private TrainingSaveData _currentSaveData;
    private AddressableTrainingEventDataRepository _addressableTrainingEventDataRepository;
    private AddressableCharacterEventDataRepository _addressableCharacterEventDataRepository;
    private AddressableSupportCardEventDataRepository _addressableSupportCardEventDataRepository;
    private AddressableTrainingEventScenarioDataRepository _trainingEventScenarioDataRepository;
    private AddressableCharacterEventScenarioDataRepository _characterEventScenarioDataRepository;
    private AddressableSupportCardEventScenarioDataRepository _supportCardEventScenarioDataRepository;

    [Inject]
    public TrainingEventDataGenerator
        (
        JsonTrainingSaveDataRepository currentSaveData,
        AddressableTrainingEventDataRepository addressableTrainingEventDataRepository,
        AddressableCharacterEventDataRepository addressableCharacterEventDataRepository,
        AddressableSupportCardEventDataRepository addressableSupportCardEventDataRepository,
        AddressableTrainingEventScenarioDataRepository trainingEventScenarioDataRepository,
        AddressableCharacterEventScenarioDataRepository characterEventScenarioDataRepository,
        AddressableSupportCardEventScenarioDataRepository supportCardEventScenarioDataRepository)
    {
        _currentSaveData = currentSaveData.RepositoryData;
        _addressableTrainingEventDataRepository = addressableTrainingEventDataRepository;
        _addressableCharacterEventDataRepository = addressableCharacterEventDataRepository;
        _addressableSupportCardEventDataRepository = addressableSupportCardEventDataRepository;
        _trainingEventScenarioDataRepository = trainingEventScenarioDataRepository;
        _characterEventScenarioDataRepository = characterEventScenarioDataRepository;
        _supportCardEventScenarioDataRepository = supportCardEventScenarioDataRepository;
    }


    public static TrainingEventData GenerateEventData(string[] targetArr)
    {
        TrainingEventData trainingEventData = new TrainingEventData();

        trainingEventData.Init(
            targetArr[0],
            targetArr[1],
            targetArr[2],
            targetArr[3],
            targetArr[4],
            targetArr[5],
            targetArr[6],
            targetArr[7],
            targetArr[8],
            targetArr[9],
            targetArr[10],
            targetArr[11],
            targetArr[12]);

        return trainingEventData;
    }

    /// <summary> イベントデータを生成する処理 </summary>
    public TrainingEventData GenerateTrainingEvent(TrainingEventType trainingEventType, uint eventID)
    {
        switch (trainingEventType)
        {
            case TrainingEventType.TrainingEvent:
                return _addressableTrainingEventDataRepository.GetCsvEventData(eventID);

            case TrainingEventType.CharacterUniqueEvent:
                return _addressableCharacterEventDataRepository.GetCsvEventData(_currentSaveData.TrainingCharacterData.CharacterID, eventID);

            case TrainingEventType.SupportCardEvent:
                return _addressableSupportCardEventDataRepository.GetCsvEventData(eventID);
        }
        return null;
    }

    /// <summary> 分岐イベントデータを生成する処理 </summary>
    public List<TrainingEventData> GenerateBranchEvent(TrainingEventType trainingEventType, uint eventID)
    {
        switch (trainingEventType)
        {
            case TrainingEventType.TrainingEvent:
                return _addressableTrainingEventDataRepository.GetBranchEventData(eventID);

            case TrainingEventType.CharacterUniqueEvent:
                return _addressableCharacterEventDataRepository.GetBranchEventData(_currentSaveData.TrainingCharacterData.CharacterID, eventID);

            case TrainingEventType.SupportCardEvent:
                return _addressableSupportCardEventDataRepository.GetBranchEventData(eventID);
        }
        return null;
    }

    /// <summary> シナリオデータを生成する処理 </summary>
    public ScenarioData GenerateScenarioData(TrainingEventType trainingEventType, uint eventID)
    {
        switch (trainingEventType)
        {
            case TrainingEventType.TrainingEvent:
                return _trainingEventScenarioDataRepository.GetScenarioData(eventID);

            case TrainingEventType.CharacterUniqueEvent:
                return _characterEventScenarioDataRepository.GetScenarioData
                    (_currentSaveData.TrainingCharacterData.CharacterID, eventID);

            case TrainingEventType.SupportCardEvent:
                return _supportCardEventScenarioDataRepository.GetScenarioData(eventID);
        }

        return null;
    }
}
