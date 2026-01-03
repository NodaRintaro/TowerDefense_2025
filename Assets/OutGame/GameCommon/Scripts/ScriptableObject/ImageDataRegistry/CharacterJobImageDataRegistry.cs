using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// Rankの画像データをまとめて管理するScriptableObjectClass
/// </summary>
[CreateAssetMenu(fileName = "CharacterJobImageDataRegistry", menuName = "ScriptableObject/ImageData/CharacterJobImageDataRegistry")]
public class CharacterJobImageDataRegistry : DataRegistryBase<JobImageData>
{
    [Inject]
    public CharacterJobImageDataRegistry()
    {

    }

    public Sprite GetData(JobType jobType)
    {
        foreach (var item in DataHolder)
        {
            if (item.JobData == jobType)
            {
                return item.SpriteData;
            }
        }
        return null;
    }
}
