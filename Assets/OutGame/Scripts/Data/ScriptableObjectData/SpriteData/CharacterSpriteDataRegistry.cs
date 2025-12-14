using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteData;

public class CharacterSpriteDataRegistry : DataRegistryBase<CharacterSprite>
{
    /// <summary>
    /// キャラクターの画像データ取得関数
    /// </summary>
    /// <param name="id"> 取得したいキャラクターのID </param>
    public Sprite GetSprite(uint id)
    {
        foreach (var data in _dataHolder)
        {
            if (id == data.CharacterID)
            {
                return data.Sprite;
            }
        }

        return null;
    }
}
