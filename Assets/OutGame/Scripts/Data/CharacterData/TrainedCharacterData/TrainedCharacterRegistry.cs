using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "TrainedCharacterRegistry", menuName = "ScriptableObject/TrainedCharacterRegistry")]
public class TrainedCharacterRegistry : ScriptableObject
{
    [SerializeField, Header("育成済みキャラクターのデータリスト")]
    private List<TrainedCharacterData> _trainedCharacterDataList = new();

    public List<TrainedCharacterData> TrainedCharacterDataList => _trainedCharacterDataList;
}
