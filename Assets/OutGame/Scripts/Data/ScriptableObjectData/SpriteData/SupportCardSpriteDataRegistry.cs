using SpriteData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "SupportCardResource", menuName = "ScriptableObject/SupportCardResource")]
public class SupportCardSpriteDataRegistry : DataRegistryBase<SupportCardSprite>
{
    /// <summary>
    /// サポートカードの画像データ取得関数
    /// </summary>
    /// <param name="id"> 取得したいサポートカードのID </param>
    /// <returns></returns>
    public Sprite GetSprite(uint id)
    {
        foreach (var data in _dataHolder)
        {
            if (id == data.CardID)
            {
                return data.Sprite;
            }
        }

        return null;
    }
}
