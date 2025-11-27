using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;


/// <summary>
/// TrainingCharcterに関するデータを保持するClass
/// </summary>
public class TrainingDataHolder
{
    private TrainingCharacterData _trainingCharacterData = null;

    private SupportCardData[] _selectedSupportCardData = null;

    public TrainingCharacterData TrainingCharacterData => _trainingCharacterData;
    public SupportCardData[] SelectedSupportCards => _selectedSupportCardData;

    /// <summary>
    /// キャラクターのデータをセット
    /// </summary>
    public void SetCharacterData(CharacterBaseData characterData)
    {
        _trainingCharacterData = new TrainingCharacterData(characterData);

        _trainingCharacterData.TakeBreak(_trainingCharacterData.MaxStamina);
    }

    public void SetSupportCardsData(SupportCardData[] supportCardDatas)
    {
        _selectedSupportCardData = supportCardDatas;
    }
}