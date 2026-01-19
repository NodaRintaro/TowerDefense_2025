using UnityEngine;
using ScenarioData;

/// <summary>
/// ノベルイベントのデータをまとめて管理するScriptableObjectClass
/// </summary>
[CreateAssetMenu(fileName = "NovelEventData", menuName = "ScriptableObject/NovelEventData")]
public class TrainingEventScenarioDataRegistry : DataRegistryBase<ScenarioData.ScenarioData>
{
    public ScenarioData.ScenarioData GetData(uint id)
    {
        foreach (var item in _dataHolder)
        {
            if (item.EventID == id)
            {
                return item;
            }
        }
        return null;
    }
}
