using UnityEngine;
using NovelData;

/// <summary>
/// ノベルイベントのデータをまとめて管理するScriptableObjectClass
/// </summary>
[CreateAssetMenu(fileName = "NovelEventData", menuName = "ScriptableObject/NovelEventData")]
public class NovelEventDataRegistry : DataRegistryBase<NovelEventData>
{
    public NovelEventData GetData(uint id)
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
