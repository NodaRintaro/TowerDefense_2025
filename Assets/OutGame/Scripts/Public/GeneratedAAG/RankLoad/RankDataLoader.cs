using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class RankDataLoader
{
    private const string _rankDataPath = "RankData/RankData";

    [Inject]
    public RankDataLoader() { }

    public async UniTask<RankDataHolder> RankDataLoad()
    {
        var request = Resources.LoadAsync<RankDataHolder>(_rankDataPath);
        await request;
        return request.asset as RankDataHolder;
    }
}
