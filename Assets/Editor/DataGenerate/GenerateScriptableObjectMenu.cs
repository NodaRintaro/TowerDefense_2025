using Cysharp.Threading.Tasks;
using ScenarioData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

#if UNITY_EDITOR
public class GenerateScriptableObjectMenu : EditorWindow
{
    private string _gasUrl = "";
    private string _scriptableObjectName = "";
    private string _dataSaveFilePath = "";
    private string _outPutCsvFilePath = "";
    private string _scriptableObjectFilePath = "";

    private DataType _dataType;

    private int _spaceSize = 10;

    [MenuItem("Tools/GenerateScriptableObject")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GenerateScriptableObjectMenu));
    }

    async void OnGUI()
    {
        //生成するDataを選択
        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("生成するDataのGASURL");
        _dataType = (DataType)EditorGUILayout.EnumPopup("DataType", _dataType);

        //生成するDataを選択
        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("生成するDataのGASURL");
        _gasUrl = EditorGUILayout.TextField("GASURL", _gasUrl);


        //生成したScriptableObjectの保存先のPath
        _scriptableObjectFilePath = _dataSaveFilePath + "/" + _scriptableObjectName + ".asset";

        // GASから取得したCSVデータを保存するファイル名のパスを設定
        _outPutCsvFilePath = _dataSaveFilePath + "/" + _scriptableObjectName + "CSV" + ".csv";

        EditorGUILayout.Space(_spaceSize);

        //スクリプタブルオブジェクトとCSVデータを両方生成する処理
        if (GUILayout.Button("GenerateScriptableObject & CSV") && NullCheckData())
        {
            await DataGenerate(true);
        }

        //CSVDataのみを生成する処理
        if (GUILayout.Button("Generate OnlyCSVData") && NullCheckData())
        {
            await DataGenerate(false);
        }
    }

    private bool NullCheckData()
    {
        if (_scriptableObjectFilePath == null) return false;
        else if (_gasUrl == null) return false;
        else if (_scriptableObjectName == null) return false;
        else if (_dataType == DataType.None) return false;

        return true;
    }

    #region データ生成
    private async UniTask DataGenerate(bool isGenerateScriptableObject)
    {
        switch (_dataType)
        {
            case DataType.CharacterData:
                _dataSaveFilePath = "Assets/MasterData/ScriptableObject/CharacterData";
                _scriptableObjectName = "CharacterDataRegistry";
                break;
            case DataType.SupportCard:
                _dataSaveFilePath = "Assets/MasterData/ScriptableObject/SupportCard";
                _scriptableObjectName = "SupportCardDataRegistry";
                break;
            case DataType.TrainingCommonEventData:
                _dataSaveFilePath = "Assets/MasterData/CSV/EventData";
                _scriptableObjectName = "TrainingCommonEventData";
                break;
            case DataType.TrainingCommonEventScenarioData:
                _dataSaveFilePath = "Assets/MasterData/CSV/ScenarioData";
                _scriptableObjectName = "TrainingCommonEventScenarioData";
                break;
            case DataType.TrainingCommonBranchEventData:
                _dataSaveFilePath = "Assets/MasterData/CSV/EventData";
                _scriptableObjectName = "TrainingCommonEventBranchEventData";
                break;
            case DataType.CharacterEventData:
                _dataSaveFilePath = "Assets/MasterData/CSV/EventData";
                _scriptableObjectName = "CharacterEventData";
                break;
            case DataType.CharacterBranchEventData:
                _dataSaveFilePath = "Assets/MasterData/CSV/EventData";
                _scriptableObjectName = "CharacterBranchEventData";
                break;
            case DataType.CharacterNovelData:
                _dataSaveFilePath = "Assets/MasterData/CSV/ScenarioData";
                _scriptableObjectName = "CharacterEventScenarioData";
                break;
            case DataType.SupportCardEventData:
                _dataSaveFilePath = "Assets/MasterData/CSV/EventData";
                _scriptableObjectName = "SupportCardEventData";
                break;
            case DataType.SupportCardBranchEventData:
                _dataSaveFilePath = "Assets/MasterData/CSV/EventData";
                _scriptableObjectName = "SupportCardBranchEventData";
                break;
            case DataType.SupportCardNovelData:
                _dataSaveFilePath = "Assets/MasterData/CSV/ScenarioData";
                _scriptableObjectName = "SupportCardScenarioData";
                break;
            case DataType.CharacterTrainingEventMap:
                _dataSaveFilePath = "Assets/MasterData/ScriptableObject/CharacterEventSchedule";
                _scriptableObjectName = "CharacterTrainingSchedule";
                break;
            case DataType.SkillData:
                _dataSaveFilePath = "Assets/MasterData/ScriptableObject/SkillData";
                _scriptableObjectName = "SkillData";
                break;
        }

        using (UnityWebRequest request = UnityWebRequest.Get(_gasUrl))
        {
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var csvData = request.downloadHandler.text;

                // 先頭の\uFEFFを削除
                if (csvData[0] == '\uFEFF')
                {
                    csvData = csvData.Substring(1);
                }

                // CSVファイルとして保存
                SaveCsvFile(csvData);

                // GASから受け取ったCSVデータを2次元配列に変換
                var parseCsvData = ParseCsv(csvData);

                if (isGenerateScriptableObject)
                {
                    // ScriptableObjectを生成
                    await DataGenerate(_dataType, parseCsvData);
                }
            }
        }
    }
    #endregion

    #region CSVデータの生成
    // CSVファイルに保存する
    private void SaveCsvFile(string data)
    {
        // if (!Directory.Exists(_outPutCsvFilePath))
        // {
        //     Directory.CreateDirectory(_outPutCsvFilePath);
        //     Debug.Log($"Created folder: {_outPutCsvFilePath}");
        // }

        File.WriteAllText(_outPutCsvFilePath, data, Encoding.UTF8);
        AssetDatabase.Refresh();
    }

    /// <summary> CSVデータを2次元配列に変換する関数 </summary>
    /// <param name="csvData">  取得したCSVのデータ </param>
    /// <returns> 取得したCSVデータを格納した2次元配列 </returns>
    string[,] ParseCsv(string csvData)
    {
        // 行ごとに分割（\r\n か \n を考慮）
        string[] rows = csvData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        // 1行目の列数を基準にする
        string[] firstRow = SplitCsvLine(rows[0]);
        int rowCount = rows.Length;
        int colCount = firstRow.Length;

        // 2次元配列を作成
        string[,] result = new string[rowCount, colCount];

        for (int i = 0; i < rowCount; i++)
        {
            string[] cols = SplitCsvLine(rows[i]);

            for (int j = 0; j < colCount; j++)
            {
                // 配列の範囲を超えた場合は空文字をセット
                result[i, j] = j < cols.Length ? cols[j] : "";
            }
        }

        return result;
    }

    /// <summary> CSVの1行を分割する関数 </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    string[] SplitCsvLine(string line)
    {
        // 正規表現でCSVのフィールドを抽出
        MatchCollection matches = Regex.Matches(line, "\"([^\"]*)\"|([^,]+)");

        string[] fields = new string[matches.Count];

        for (int i = 0; i < matches.Count; i++)
        {
            fields[i] = matches[i].Value.Trim('"'); // ダブルクォートを削除
        }

        return fields;
    }

    /// <summary> 外部から生成したCSVファイルを2次元配列でロードする関数 </summary>
    /// <param name="filePath"> 生成先のPath </param>
    public static string[,] LoadCsvAs2DArray(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath); // 行ごとに読み込む
        int rows = lines.Length;
        int cols = lines[0].Split(',').Length; // 1行目の列数を基準にする
        string[,] array = new string[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            string[] cells = lines[i].Split(','); // カンマ区切りで分割
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = cells[j].Trim('\"'); // 余分な " を削除
            }
        }

        return array;
    }
    #endregion

    #region スクリプタブルオブジェクト生成
    public async UniTask DataGenerate(DataType dataType, string[,] parseCsvData)
    {
        switch (dataType)
        {
            case DataType.CharacterData:
                _scriptableObjectName = "CharacterDataRegistry";
                GenerateCharacterData(parseCsvData);
                break;
            case DataType.SupportCard:
                _scriptableObjectName = "SupportCardDataRegistry";
                GenerateSupportCardData(parseCsvData);
                break;
            case DataType.CharacterTrainingEventMap:
                _scriptableObjectName = "CharacterTrainingSchedule";
                GenerateCharacterTrainingScheduleData(parseCsvData);
                break;
            case DataType.SkillData:
                _scriptableObjectName = "SkillDataRegistry";
                await GenerateSkillData(parseCsvData);
                break;
        }
    }
    #endregion

    #region キャラクターデータ生成
    /// <summary> キャラクターのスクリプタブルオブジェクトを生成 </summary>
    /// <param name="parseCsvData"> 2次元配列に格納されたキャラクターのCSVデータ </param>
    private void GenerateCharacterData(string[,] parseCsvData)
    {
        const int firstColumnCountNum = 2;
        int csvDataLength = parseCsvData.GetLength(0);
        CharacterBaseDataRegistry characterDataList = CreateInstance<CharacterBaseDataRegistry>();

        CharacterBaseData[] characterBaseDataArray = new CharacterBaseData[csvDataLength - firstColumnCountNum];

        for (int columnCount = firstColumnCountNum; columnCount < csvDataLength; columnCount++)
        {
            CharacterBaseData characterData = new();

            characterData.InitData(
                uint.Parse(parseCsvData[columnCount, 0]),
                parseCsvData[columnCount, 1],
                uint.Parse(parseCsvData[columnCount, 2]),
                uint.Parse(parseCsvData[columnCount, 3]),
                uint.Parse(parseCsvData[columnCount, 4]),
                uint.Parse(parseCsvData[columnCount, 5]),
                uint.Parse(parseCsvData[columnCount, 6]),
                parseCsvData[columnCount, 7],
                uint.Parse(parseCsvData[columnCount, 8])
                );

            characterBaseDataArray[columnCount - firstColumnCountNum] = characterData;
        }

        characterDataList.InitData(characterBaseDataArray);
        AssetDataCreate(characterDataList);
    }
    #endregion

    #region サポートカードデータ生成
    /// <summary> サポートカードのスクリプタブルオブジェクトを生成 </summary>
    /// <param name="parseCsvData"> 2次元配列に格納されたサポートカードのCSVデータ </param>
    private void GenerateSupportCardData(string[,] parseCsvData)
    {
        const int firstColumnCountNum = 2;
        int csvDataLength = parseCsvData.GetLength(0);
        SupportCardDataRegistry supportCardDataRegistry = CreateInstance<SupportCardDataRegistry>();

        SupportCardData[] supportCardDataArray = new SupportCardData[csvDataLength - firstColumnCountNum];


        for (int columnCount = 2; columnCount < parseCsvData.GetLength(0); columnCount++)
        {
            SupportCardData cardData = new();

            cardData.InitData(
                parseCsvData[columnCount, 0],
                parseCsvData[columnCount, 1],
                parseCsvData[columnCount, 2],
                parseCsvData[columnCount, 3],
                parseCsvData[columnCount, 4],
                parseCsvData[columnCount, 5],
                parseCsvData[columnCount, 6],
                parseCsvData[columnCount, 7],
                parseCsvData[columnCount, 8],
                parseCsvData[columnCount, 9],
                parseCsvData[columnCount, 10],
                parseCsvData[columnCount, 11],
                parseCsvData[columnCount, 12]
                );

            supportCardDataArray[columnCount - firstColumnCountNum] = cardData;
        }

        supportCardDataRegistry.InitData(supportCardDataArray);
        AssetDataCreate(supportCardDataRegistry);
    }
    #endregion

    #region キャラクター各種の育成予定データ
    /// <summary> キャラクター各種の育成予定データのスクリプタブルオブジェクトを生成 </summary>
    /// <param name="parseCsvData"> 2次元配列に格納されたサポートカードのCSVデータ </param>
    private void GenerateCharacterTrainingScheduleData(string[,] parseCsvData)
    {
        int csvDataLength = parseCsvData.GetLength(0);
        CharacterTrainingScheduleRegistry characterTrainingEventMapRegistry = CreateInstance<CharacterTrainingScheduleRegistry>();

        CharacterTrainingEventMap[] characterTrainingEventMaps = null;

        int arrCount = 0;
        for (int i = 2; i < parseCsvData.GetLength(0); i++)
        {
            if (!string.IsNullOrWhiteSpace(parseCsvData[i, 0]))
                arrCount++;
        }
        characterTrainingEventMaps = new CharacterTrainingEventMap[arrCount];

        List<OneDayEvent> trainingSchedule = null;
        arrCount = 0;

        for (int i = 2; i < parseCsvData.GetLength(0); i++)
        {
            if (!string.IsNullOrWhiteSpace(parseCsvData[i, 0]))
            {
                if (trainingSchedule != null)
                {
                    characterTrainingEventMaps[arrCount].SetCharacterTrainingSchedule(trainingSchedule.ToArray());
                    arrCount++;
                }

                trainingSchedule = new();
                characterTrainingEventMaps[arrCount] = new();
                characterTrainingEventMaps[arrCount].SetID(uint.Parse(parseCsvData[i, 0]));
            }
            else
            {
                OneDayEvent oneDayEvent = new OneDayEvent();

                oneDayEvent.IsRaid = bool.Parse(parseCsvData[i, 2]);

                if (!string.IsNullOrWhiteSpace(parseCsvData[i, 3]))
                    oneDayEvent.FirstUniqueEvent = uint.Parse(parseCsvData[i, 3]);

                if (!string.IsNullOrWhiteSpace(parseCsvData[i, 4]))
                    oneDayEvent.SecondUniqueEvent = uint.Parse(parseCsvData[i, 4]);

                if (!string.IsNullOrWhiteSpace(parseCsvData[i, 5]))
                    oneDayEvent.LastUniqueEvent = uint.Parse(parseCsvData[i, 5]);

                trainingSchedule.Add(oneDayEvent);
            }
        }

        characterTrainingEventMaps[arrCount].SetCharacterTrainingSchedule(trainingSchedule.ToArray());
        characterTrainingEventMapRegistry.InitData(characterTrainingEventMaps);
        AssetDataCreate(characterTrainingEventMapRegistry);
    }
    #endregion

    #region スキルデータ生成
    public async UniTask GenerateSkillData(string[,] parseCsvData)
    {
        const int firstColumnCountNum = 2;
        int csvDataLength = parseCsvData.GetLength(0);
        SkillDataRegistry skillDataRegistry = CreateInstance<SkillDataRegistry>();

        SkillData[] skillArray = new SkillData[csvDataLength - firstColumnCountNum];

        for (int columnCount = firstColumnCountNum; columnCount < csvDataLength; columnCount++)
        {
            SkillData skillData = new();

            // Sprite icon = await AssetsLoader.LoadAssetAsync<Sprite>(parseCsvData[columnCount, 3]);
            // GameObject prefab = await AssetsLoader.LoadAssetAsync<GameObject>(parseCsvData[columnCount, 4]);

            skillData.InitData(
                uint.Parse(parseCsvData[columnCount, 0]),
                parseCsvData[columnCount, 1],
                parseCsvData[columnCount, 2],
                null,
                null,
                bool.Parse(parseCsvData[columnCount, 5]),
                Enum.Parse<DurationType>(parseCsvData[columnCount, 6]),
                float.Parse(parseCsvData[columnCount, 7]),
                Enum.Parse<CoolTimeRecoveryType>(parseCsvData[columnCount, 8]),
                float.Parse(parseCsvData[columnCount, 9]),
                int.Parse(parseCsvData[columnCount, 10])
                );

            skillArray[columnCount - firstColumnCountNum] = skillData;
        }

        skillDataRegistry.InitData(skillArray);
        AssetDataCreate(skillDataRegistry);
    }
    #endregion

    private void AssetDataCreate(UnityEngine.Object data)
    {
        // アセットとして保存
        AssetDatabase.CreateAsset(data, _scriptableObjectFilePath);
        AssetDatabase.SaveAssets();

        //AssetDataBaseの内容を更新
        AssetDatabase.Refresh();
    }
}
#endif