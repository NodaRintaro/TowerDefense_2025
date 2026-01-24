using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Jsonのデータを保存するロジッククラス
/// </summary>
public static class JsonDataSaveSystem
{
    private static readonly byte[] keys = { 0xfe, 0x80, 0xfe, 0x80 };

    /// <summary> Pathを指定してDataを非同期でセーブする </summary>
    public static async UniTask DataSaveAsync<T>(T data, string saveDataName) where T : IJsonSaveData
    {
        string filePath = Application.persistentDataPath + "/" + saveDataName + ".json";
        string json = JsonConvert.SerializeObject(data);
        byte[] encodeJson = EncodeText(json);
        // 非同期でバイト配列を書き込む
        await File.WriteAllBytesAsync(filePath, encodeJson);

        //// 暗号化してない状態での保存
        //await File.WriteAllTextAsync(filePath, json); // 非同期版
    }

    /// <summary> 保存先のパスからDataを非同期でLoadする</summary>
    public static async UniTask<T> DataLoadAsync<T>(string saveDataName) where T : IJsonSaveData
    {
        string filePath = Application.persistentDataPath + "/" + saveDataName + ".json";

        if (File.Exists(filePath))
        {
            // 非同期でバイト配列を読み込む
            byte[] encodeJson = await File.ReadAllBytesAsync(filePath);
            string json = DecodeBytes(encodeJson);
            T loaded = JsonConvert.DeserializeObject<T>(json);
            return loaded;
        }

        //// 暗号化してない状態でのLoad
        //if (File.Exists(filePath))
        //{
        //    string encodeJson = await File.ReadAllTextAsync(filePath); // 非同期版
        //    T loaded = JsonConvert.DeserializeObject<T>(encodeJson);
        //    return loaded;
        //}

        //何もなければデフォルト値を返す
        return default(T);
    }

    public static async UniTask DataInitialize(string dataName)
    {
        string streamingFilePath = Application.streamingAssetsPath + "/" + dataName + ".json";

        string persistentFilePath = Application.persistentDataPath + "/" + dataName + ".json";

        if (File.Exists(streamingFilePath))
        {
            // 非同期でバイト配列を読み込む
            byte[] encodeJson = await File.ReadAllBytesAsync(streamingFilePath);


            if(!File.Exists(persistentFilePath))
                // 非同期でバイト配列を書き込む
                await File.WriteAllBytesAsync(persistentFilePath, encodeJson);
        }
    }

    /// <summary> Pathを指定してDataを非同期でStreamingAssetsにセーブする </summary>
    public static async UniTask DataSaveStreamingAssetsAsync<T>(T data, string saveDataName) where T : IJsonSaveData
    {
        string filePath = Application.streamingAssetsPath + "/" + saveDataName + ".json";
        string json = JsonConvert.SerializeObject(data);
        byte[] encodeJson = JsonDataSaveSystem.EncodeText(json);
        // 非同期でバイト配列を書き込む
        await File.WriteAllBytesAsync(filePath, encodeJson);
    }

    /// <summary> StreamingAssetsの保存先のパスからDataを非同期でLoadする</summary>
    public static async UniTask<T> DataLoadAsyncStreamingAssetsAsync<T>(string saveDataName) where T : IJsonSaveData
    {
        string filePath = Application.streamingAssetsPath + "/" + saveDataName + ".json";

        if (File.Exists(filePath))
        {
            // 非同期でバイト配列を読み込む
            byte[] encodeJson = await File.ReadAllBytesAsync(filePath);
            string json = DecodeBytes(encodeJson);
            T loaded = JsonConvert.DeserializeObject<T>(json);
            return loaded;
        }
        return default(T);
    }

    public static void DataDelete(string saveDataName)
    {
        string filePath = Application.persistentDataPath + "/" + saveDataName + ".json";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);  // ファイルを完全に削除
            Debug.Log($"削除完了: {filePath}");
        }
    }

    /// <summary> テキストを難読化 </summary>
    private static byte[] EncodeText(string text)
    {
        // string -> byte[] にしてから難読化
        byte[] byteArray = Encoding.UTF8.GetBytes(text);
        DecodeEncode(ref byteArray);
        return byteArray;
    }

    /// <summary> 難読化したテキストを復元 </summary>
    public static string DecodeBytes(byte[] byteArray)
    {
        DecodeEncode(ref byteArray);
        return Encoding.UTF8.GetString(byteArray);
    }

    /// <summary> 難読化 </summary>
    private static void DecodeEncode(ref byte[] byteArray)
    {
        for (int i = 0; i < byteArray.Count(); i++)
        {
            byteArray[i] ^= keys[i % 4];
        }
    }


}
