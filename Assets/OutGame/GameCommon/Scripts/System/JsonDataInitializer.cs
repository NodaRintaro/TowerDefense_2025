using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.Networking; // Android等での読み込みに必要

public class DataInitializer : MonoBehaviour
{
    public void Start()
    {
        InitData();
    }

    public async void InitData()
    {
        await JsonDataSaveSystem.DataInitialize(JsonCharacterCollectionDataRepository.SaveDataName);
        await JsonDataSaveSystem.DataInitialize(JsonSupportCardCollectionDataRepository.SaveDataName);
        await JsonDataSaveSystem.DataInitialize(JsonTowerDefenseCharacterDataRepository.SaveDataName);
        await JsonDataSaveSystem.DataInitialize(JsonCharacterDeckDataRepository.SaveDataName);
    }
}