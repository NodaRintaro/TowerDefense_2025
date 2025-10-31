using VContainer;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Text;

public class TrainingCharacterSaveDataManager
{
    private const string _saveDataName = "/TrainingData.json";
    private string _filePath;

    [Inject]
    public TrainingCharacterSaveDataManager()
    {
        _filePath = Application.persistentDataPath + _saveDataName;
    }

    public void DataSave(TrainingDataHolder trainingData)
    {
        string json = JsonConvert.SerializeObject(trainingData);
        File.WriteAllText(_filePath, json);
    }

    public TrainingDataHolder DataLoad()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            TrainingDataHolder loaded = JsonConvert.DeserializeObject<TrainingDataHolder>(json);
            return loaded;
        }

        return null;
    }
}
