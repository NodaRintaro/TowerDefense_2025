using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rankの画像データをまとめて管理するScriptableObjectClass
/// </summary>
[CreateAssetMenu(fileName = "RankImageDataRegistry", menuName = "ScriptableObject/ImageData/RankImageDataRegistry")]
public class RankImageDataRegistry : DataRegistryBase<RankImageData>
{
    public Sprite GetData(RankType rankType)
    {
        foreach (var item in DataHolder)
        {
            if(item.RankType == rankType)
            {
                return item.SpriteData;
            }
        }
        return null;
    }
}
