using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// トレーニングで使用するデータを生成するクラス
/// </summary>
public class TrainingDataCreater
{
    private TrainingDataHolder _trainingData = null;

    private SupportCardData[] _selectedSupportCardData = null;

    private const int _supportCardDeckNum = 4;

    [Inject]
    public TrainingDataCreater()
    {
        _trainingData = new();
        _selectedSupportCardData = new SupportCardData[_supportCardDeckNum];
    }

    /// <summary> トレーニングするキャラクターの選択 </summary>
    public void SetTrainingCharacter(CharacterBaseData characterBaseData)
    {
        _trainingData.SetCharacterData(characterBaseData);
    }

    /// <summary> トレーニングで使用するサポートカードをセット </summary>
    public void SetSupportCard(int cardDeckNum, SupportCardData supportCardData)
    {
        _selectedSupportCardData[cardDeckNum] = supportCardData;
    }
}
