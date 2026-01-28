using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class TrainingEventSelector
{
    private JsonTrainingSaveDataRepository _trainingSaveDataRepository;
    private TrainingEventPool _trainingEventPool;
    private AddressableSupportCardEventDataRepository _addressableSupportCardEventDataRepository;

    [Inject]
    public TrainingEventSelector(JsonTrainingSaveDataRepository jsonTrainingSaveDataRepository,
        TrainingEventPool trainingEventPool, AddressableSupportCardEventDataRepository addressableSupportCardEventDataRepository)
    {
        _trainingSaveDataRepository = jsonTrainingSaveDataRepository;
        _trainingEventPool = trainingEventPool;
        _addressableSupportCardEventDataRepository = addressableSupportCardEventDataRepository;
    }

    public void SetTrainingCommonEvent(uint eventID)
    {
        _trainingEventPool.EnqueueData(TrainingEventType.TrainingEvent, eventID);
    }

    public void SetCharacterUniqueEvent()
    {
        CharacterTrainingSchedule characterTrainingSchedule = _trainingSaveDataRepository.RepositoryData.CurrentCharacterSchedule;
        OneDayEvents oneDayEvents = characterTrainingSchedule.TrainingEventSchedule[_trainingSaveDataRepository.RepositoryData.CurrentElapsedDays];
        if (oneDayEvents.IsRaid)
        {
            _trainingEventPool.EnqueueData(TrainingEventType.CharacterUniqueEvent, oneDayEvents.FirstUniqueEvent);
            _trainingEventPool.EnqueueData(TrainingEventType.CharacterUniqueEvent, oneDayEvents.LastUniqueEvent);
        }
        else
        {
            _trainingEventPool.EnqueueData(TrainingEventType.CharacterUniqueEvent, oneDayEvents.FirstUniqueEvent);
            _trainingEventPool.EnqueueData(TrainingEventType.CharacterUniqueEvent, oneDayEvents.SecondUniqueEvent);
            _trainingEventPool.EnqueueData(TrainingEventType.CharacterUniqueEvent, oneDayEvents.LastUniqueEvent);
        }
    }

    public void SetSupportCardEvent()
    {
        List<TrainingEventData> cardEventList = new List<TrainingEventData>();
        foreach (var card in _trainingSaveDataRepository.RepositoryData.TrainingCardDeckData.CardDeckData)
        {
            cardEventList.AddRange(_addressableSupportCardEventDataRepository.GetSupportCardTrainingEventDataList(card.ID));
        }

        TrainingEventData selectedEvent = SelectRandomSupportCardEvent(cardEventList);
        _trainingEventPool.EnqueueData(TrainingEventType.SupportCardEvent, selectedEvent.EventID);
    }

    /// <summary>
    /// 複数のListから全要素を集めて、その中から無作為に1つを選択する
    /// </summary>
    private TrainingEventData SelectRandomSupportCardEvent(List<TrainingEventData> eventList)
    {
        // 要素が存在しない場合の処理
        if (eventList.Count == 0)
        {
            throw new InvalidOperationException("全てのListが空です");
        }

        // 無作為に1つ選択
        System.Random random = new System.Random();
        int index = random.Next(eventList.Count);
        return eventList[index];
    }
}
