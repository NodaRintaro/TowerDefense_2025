using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;

public static class DataSaveSystem
{
    /// <summary> Pathを指定してDataをセーブする </summary>
    public static void DataSave<T>(T data, string saveDataName)
    {
        string filePath = Application.persistentDataPath + "/" + saveDataName + ".json";
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(filePath, json);
    }

    /// <summary> 保存先のパスからLoadするObjectを取得 </summary>
    public static T DataLoad<T>(string saveDataName)
    {
        string filePath = Application.persistentDataPath + "/" + saveDataName + ".json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            T loaded = JsonConvert.DeserializeObject<T>(json);
            return loaded;
        }

        //何もなければデフォルト値を返す
        return default(T);
    }
}
