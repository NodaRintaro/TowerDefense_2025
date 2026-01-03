using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの画像データをまとめて管理するScriptableObjectClass
/// </summary>
[CreateAssetMenu(fileName = "CharacterDataRegistry", menuName = "ScriptableObject/ImageData/CharacterDataRegistry")]
public class CharacterImageDataRegistry : DataRegistryBase<CharacterImageData>
{
    /// <summary> キャラクターのイメージデータの取得関数 </summary>
    public CharacterImageData GetImageData(uint id)
    {
        foreach (var item in DataHolder)
        {
            if (item.ID == id)
            {
                return item;
            }
        }
        return null;
    }

    /// <summary> キャラクターの特定のスプライト画像の取得関数 </summary>
    public Sprite GetCharacterSprite(uint id, CharacterSpriteType spriteType)
    {
        foreach (var item in DataHolder)
        {
            if (item.ID == id)
            {
                return item.GetSprite(spriteType);
            }
        }
        return null;
    }
}
