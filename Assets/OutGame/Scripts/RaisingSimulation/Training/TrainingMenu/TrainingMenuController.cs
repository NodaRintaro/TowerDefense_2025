using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// トレーニングイベントの入力を受けてトレーニングへの移行を行うクラス
/// </summary>
public class TrainingMenuController : MonoBehaviour
{
    [SerializeField, Header("キャラクターのデータの保存先")]
    private string _characterDataPath = "CharacterData";

    [SerializeField, Header("トレーニングするキャラクターのベースデータ")]
    private TrainingCharacterData _currentTrainigCharacterData = default;

    [SerializeField, Header("レイドイベント開始までのカウントダウン")]
    private int _raidEventCountDown;

    [SerializeField, Header("Trainingの種類一覧")]
    private TrainingEvent[] _trainingEventList = default;

    private int _trainingCharacterID = 0;

    private CharacterDataHolder _characterDataList = default;

    public TrainingCharacterData CurrentTrainingCharacterData => _currentTrainigCharacterData;

    public TrainingEvent[] TrainingEventList => _trainingEventList;

    private void Start()
    {
        _characterDataList = Resources.Load<CharacterDataHolder>(_characterDataPath);
    }

    public void SetCharacterID(int id) => _trainingCharacterID = id;

    public void RaidCountDown() => _raidEventCountDown--;

    public void SetRaidCount(int setNum) => _raidEventCountDown = setNum;

    /// <summary> トレーニングキャラクターの初期化 </summary>
    /// <param name="characterID"> キャラのID </param>
    public void InitTrainigCharacter(int characterID)
    {
        _currentTrainigCharacterData = new();
        _currentTrainigCharacterData.SetBaseCharacter(CharacterDataFind(characterID));
    }

    /// <summary> キャラクターのトレーニングが開始されたときの処理 </summary>
    public void StartTraining(TrainingType trainingType)
    {
        foreach (var trainingEvent in _trainingEventList) 
        {
            if(trainingEvent.TrainingType == trainingType)
            {
                trainingEvent.OnTrainingEvent(_currentTrainigCharacterData);
            }
        }
    }

    /// <summary> キャラクターのトレーニングが終了したときの処理 </summary>
    public void FinishTraining()
    {
        RaidCountDown();
    }

    /// <summary> IDからCharacterDataを探す処理 </summary>
    private CharacterData CharacterDataFind(int characterID)
    {
        CharacterData characterData = null;
        foreach (var character in _characterDataList.DataList)
        {
            if (character.ID == characterID)
            {
                characterData = character;
            }
        }
        return characterData;
    }
}

public enum TrainingType
{
    None,
    Pysical,
    Power,
    Intelligence,
    Speed,
    Rect
}