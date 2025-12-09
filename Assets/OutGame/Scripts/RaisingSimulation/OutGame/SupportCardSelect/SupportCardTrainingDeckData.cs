using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;
using SupportCardData;

[System.Serializable]
public class SupportCardTrainingDeckData
{
    [SerializeField, Header("サポートカードのデータの保存先")]
    private SupportCardDataHolder _supportCardDataHolder;

    [SerializeField, Header("サポートカードのSpriteの保存先")]
    private SupportCardSpriteData[] _supportCardResourceHolder;

    //サポートカードのデータの保存先のパス
    private const string _supportCardDataPath = "SupportCard/SupportCardData/SupportCardDataList";
    private const string _supportCardResourcePath = "SupportCard/SupportCardSprite";

    public SupportCardDataHolder SupportCardDataHolder => _supportCardDataHolder;
    public SupportCardSpriteData[] SupportCardResources => _supportCardResourceHolder;

    [Inject]
    public SupportCardTrainingDeckData(){}

    public async UniTask CardDataLoad()
    {
        _supportCardDataHolder = await CardDataLoad(_supportCardDataPath);
        _supportCardResourceHolder = await CharacterResourceLoad(_supportCardResourcePath);
    }

    private async UniTask<SupportCardDataHolder> CardDataLoad(string path)
    {
        var resource = Resources.LoadAsync<SupportCardDataHolder>(path);
        await resource;
        return resource.asset as SupportCardDataHolder;
    }

    private async UniTask<SupportCardSpriteData[]> CharacterResourceLoad(string path)
    {
        // パス配下のすべてのアセットを取得
        Object[] loadResource = Resources.LoadAll(path, typeof(SupportCardSpriteData));
        SupportCardSpriteData[] result = new SupportCardSpriteData[loadResource.Length];

        for (int i = 0; i < loadResource.Length; i++)
        {
            // ちょっとずつ非同期的に処理する（実際の読み込みは同期だが負荷を分散）
            await UniTask.Yield();

            result[i] = loadResource[i] as SupportCardSpriteData;
        }

        return result;
    }

    public CardData GetSupportCardData(uint id)
    {
        foreach(var data in _supportCardDataHolder.DataList)
        {
            if(data.ID == id)
            {
                return data;
            }
        }
        Debug.Log("IDが見つかりません");
        return null;
    }

    public SupportCardSpriteData GetCardResource(uint id)
    {
        //foreach (var data in _supportCardResourceHolder)
        //{
        //    if (data.CardID == id)
        //    {
        //        return data;
        //    }
        //}
        //Debug.Log("IDが見つかりません");
        return null;
    }
}
