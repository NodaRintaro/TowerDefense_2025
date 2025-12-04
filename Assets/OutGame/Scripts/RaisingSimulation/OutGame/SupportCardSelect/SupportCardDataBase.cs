using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

[System.Serializable]
public class SupportCardDataBase
{
    [SerializeField, Header("サポートカードのデータの保存先")]
    private SupportCardDataHolder _supportCardDataHolder;

    [SerializeField, Header("サポートカードのSpriteの保存先")]
    private SupportCardSprite[] _supportCardResourceHolder;

    //サポートカードのデータの保存先のパス
    private const string _supportCardDataPath = "SupportCard/SupportCardData/SupportCardDataList";
    private const string _supportCardResourcePath = "SupportCard/SupportCardSprite";

    public SupportCardDataHolder SupportCardDataHolder => _supportCardDataHolder;
    public SupportCardSprite[] SupportCardResources => _supportCardResourceHolder;

    [Inject]
    public SupportCardDataBase(){}

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

    private async UniTask<SupportCardSprite[]> CharacterResourceLoad(string path)
    {
        // パス配下のすべてのアセットを取得
        Object[] loadResource = Resources.LoadAll(path, typeof(SupportCardSprite));
        SupportCardSprite[] result = new SupportCardSprite[loadResource.Length];

        for (int i = 0; i < loadResource.Length; i++)
        {
            // ちょっとずつ非同期的に処理する（実際の読み込みは同期だが負荷を分散）
            await UniTask.Yield();

            result[i] = loadResource[i] as SupportCardSprite;
        }

        return result;
    }

    public SupportCardData GetSupportCardData(uint id)
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

    public SupportCardSprite GetCardResource(uint id)
    {
        foreach (var data in _supportCardResourceHolder)
        {
            if (data.CardID == id)
            {
                return data;
            }
        }
        Debug.Log("IDが見つかりません");
        return null;
    }
}
