using DG.Tweening.Plugins.Core.PathCore;
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
    static readonly byte[] keys = { 0xfe, 0x80, 0xfe, 0x80 };

    /// <summary> Pathを指定してDataをセーブする </summary>
    public static void DataSave<T>(T data, string saveDataName) where T : IJsonSaveData
    {
        string filePath = Application.persistentDataPath + "/" + saveDataName + ".json";
        string json = JsonConvert.SerializeObject(data);
        byte[] encodeJson = EncodeText(json);
        File.WriteAllBytes(filePath, encodeJson);
    }

    /// <summary> 保存先のパスからDataをLoadする</summary>
    public static T DataLoad<T>(string saveDataName) where T : IJsonSaveData
    {
        string filePath = Application.persistentDataPath + "/" + saveDataName + ".json";

        if (File.Exists(filePath))
        {
            byte[] encodeJson = File.ReadAllBytes(filePath);
            string json = DecodeBytes(encodeJson);
            T loaded = JsonConvert.DeserializeObject<T>(json);
            return loaded;
        }

        //何もなければデフォルト値を返す
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
    private static string DecodeBytes(byte[] byteArray)
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
