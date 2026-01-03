using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サポートカードの画像データをまとめて管理するScriptableObjectClass
/// </summary>
[CreateAssetMenu(fileName = "SupportCardImageDataRegistry", menuName = "ScriptableObject/ImageData/SupportCardImageDataRegistry")]
public class SupportCardImageDataRegistry : DataRegistryBase<SupportCardImageData>
{
    public Sprite GetData(uint id)
    {
        foreach (var item in DataHolder)
        {
            if (item.ID == id)
            {
                return item.SpriteData;
            }
        }
        return null;
    }
}