using Cysharp.Threading.Tasks;
using NovelData;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

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
        //TextFieldで自動生成するScriptableObjectの名前を入力
        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("自動生成するScriptableObjectの名前");
        _scriptableObjectName = EditorGUILayout.TextField("scriptableObjectName", _scriptableObjectName);

        //ほぞんさきのPathを指定
        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("保存先のPath");
        _dataSaveFilePath = EditorGUILayout.TextField("dataSaveFilePath", _dataSaveFilePath);

        //生成するDataを選択
        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("生成するDataのタイプを選択");
        _dataType = (DataType)EditorGUILayout.EnumPopup("GenerateDataType", _dataType);


        //生成したScriptableObjectの保存先のPath
        _scriptableObjectFilePath = _dataSaveFilePath + "/" + _scriptableObjectName + ".asset";

        // GASから取得したCSVデータを保存するファイル名のパスを設定
        _outPutCsvFilePath = _dataSaveFilePath + "/" + _scriptableObjectName + "CSV" + ".csv";

        EditorGUILayout.Space(_spaceSize);
        if (GUILayout.Button("GenerateScriptableObject") && NullCheckData())
        {
            SetDataPath(_dataType);
            await GenerateScriptableObject();
        }
    }

    private void SetDataPath(DataType dataType)
    {
        switch(dataType)
        {
            case DataType.CharacterData:
                _gasUrl = "https://script.google.com/macros/s/AKfycbzb7FruTOZaNcvxrKhOIXUFfpXSaqavkhW2T-5G87b9jMOjP1tgwnzxR2NtapJHWcEb/exec";
                break;
            case DataType.SupportCard:
                _gasUrl = "https://script.google.com/macros/s/AKfycbyBrmGAo-1ROKoMhnQLAMdp2M7p3pGK__x8JRxviTcR1GGA5mb9s9WZUWDnM5rECu4B/exec";
                break;
            case DataType.TrainingEventData:
                _gasUrl = "https://script.google.com/macros/s/AKfycbwowVMLaudNyg7BIdwHOFc7fUNVM6RRcOaFjhAq2nhNQlj_sfUExpbwFQ2JTUL_domk/exec";
                break;
            case DataType.NovelData:
                _gasUrl = "https://script.google.com/macros/s/AKfycbxYBJsVTBvYTYR_XfYykn6JlKYWI7pPg_JNKuPHFA23n7Fo27hNqc8ALBOnep3rdEGK/exec";
                break;
        }
    }

    private bool NullCheckData()
    {
        if (_scriptableObjectFilePath == null) return false;
        else if(_gasUrl  == null) return false;
        else if(_scriptableObjectName == null) return false;
        else if(_dataType == DataType.None) return false;

        return true;
    }

    private async UniTask GenerateScriptableObject()
    {
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

                // ScriptableObjectを生成
                DataGenerate(_dataType, parseCsvData);
            }
        }
    }

    // CSVファイルに保存する
    private void SaveCsvFile(string data)
    {
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

    public void DataGenerate(DataType dataType, string[,] parseCsvData)
    {
        switch(dataType)
        {
            case DataType.CharacterData:
                GenerateCharacterData(parseCsvData);
                break;
            case DataType.SupportCard:
                GenerateSupportCardData(parseCsvData);
                break;
            case DataType.TrainingEventData:
                GenerateTrainingEventData(parseCsvData);
                break;
            case DataType.NovelData:
                GenerateNovelEventData(parseCsvData);
                break;
        }
    }

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
                parseCsvData[columnCount, 7]
                );

            supportCardDataArray[columnCount - firstColumnCountNum] = cardData;
        }

        supportCardDataRegistry.InitData(supportCardDataArray);
        AssetDataCreate(supportCardDataRegistry);
    }

    /// <summary> トレーニングイベントのスクリプタブルオブジェクトを生成 </summary>
    /// <param name="parseCsvData"> 2次元配列に格納されたサポートカードのCSVデータ </param>
    private void GenerateTrainingEventData(string[,] parseCsvData)
    {
        const int firstColumnCountNum = 2;
        int csvDataLength = parseCsvData.GetLength(0);
        TrainingEventDataRegistry eventDataRegistry = CreateInstance<TrainingEventDataRegistry>();

        TrainingEventData[] eventDataArray = new TrainingEventData[csvDataLength - firstColumnCountNum];


        for (int columnCount = 2; columnCount < parseCsvData.GetLength(0); columnCount++)
        {
            TrainingEventData eventData = new();

            eventData.InitData(
                uint.Parse(parseCsvData[columnCount, 0]),
                parseCsvData[columnCount, 1],
                uint.Parse(parseCsvData[columnCount, 2]),
                int.Parse(parseCsvData[columnCount, 3]),
                int.Parse(parseCsvData[columnCount, 4]),
                int.Parse(parseCsvData[columnCount, 5]),
                int.Parse(parseCsvData[columnCount, 6]),
                int.Parse(parseCsvData[columnCount, 7]),
                uint.Parse(parseCsvData[columnCount, 8]),
                uint.Parse(parseCsvData[columnCount, 9])
                );

            eventDataArray[columnCount - firstColumnCountNum] = eventData;
        }

        eventDataRegistry.InitData(eventDataArray);
        AssetDataCreate(eventDataRegistry);
    }

    /// <summary> ノベルイベントのスクリプタブルオブジェクトを生成 </summary>
    /// <param name="parseCsvData"> 2次元配列に格納されたサポートカードのCSVデータ </param>
    private void GenerateNovelEventData(string[,] parseCsvData)
    {
        int csvDataLength = parseCsvData.GetLength(0);
        NovelEventDataRegistry eventDataRegistry = CreateInstance<NovelEventDataRegistry>();

        List<NovelEventData> eventDataArray = new();
        NovelEventData eventData = null;
        List<NovelData.NovelData> novelScreenDataList = null;

        for (int columnCount = 1; columnCount < parseCsvData.GetLength(0); columnCount++)
        {
            NovelData.NovelData novelData;

            if(string.IsNullOrWhiteSpace(parseCsvData[columnCount, 0]))
            {
                novelData = new NovelData.NovelData
                {
                    TalkCharacterName = parseCsvData[columnCount, 1],
                    ScenarioData = parseCsvData[columnCount, 2]
                };
                novelScreenDataList.Add(novelData);
            }
            else
            {
                if(novelScreenDataList != null && eventData != null)
                {
                    eventData.SetNovelData(novelScreenDataList.ToArray());
                    eventDataArray.Add(eventData);
                }

                novelScreenDataList = new();
                eventData = new();
                eventData.SetID(uint.Parse(parseCsvData[columnCount, 0]));

                novelData = new NovelData.NovelData
                {
                    TalkCharacterName = parseCsvData[columnCount, 1],
                    ScenarioData = parseCsvData[columnCount, 2]
                };
                novelScreenDataList.Add(novelData);
            }

            if (columnCount == parseCsvData.GetLength(0) - 1)
            {
                eventData.SetNovelData(novelScreenDataList.ToArray());
                eventDataArray.Add(eventData);
            }
        }

        eventDataRegistry.InitData(eventDataArray.ToArray());
        AssetDataCreate(eventDataRegistry);
    }

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