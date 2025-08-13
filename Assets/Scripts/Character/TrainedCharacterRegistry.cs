using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataList", menuName = "ScriptableObject/CharacterDataList")]
public class TrainedCharacterRegistry : ScriptableObject
{
    [SerializeField, Header("育成済みキャラクターのデータリスト")]
    private List<TrainedCharacterData> _trainedCharacterDataList = new();

    public List<TrainedCharacterData> TrainedCharacterDataList => _trainedCharacterDataList;
}
