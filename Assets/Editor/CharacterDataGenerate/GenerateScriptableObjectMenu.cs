using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

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

    [MenuItem("Tool/GenerateScriptableObjectMenu")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GenerateScriptableObjectMenu));
    }

    async void OnGUI()
    {
        // TextFieldでGASのURLを入力
        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("欲しいDataのGASのURL");
        _gasUrl = EditorGUILayout.TextField("gasUrl", _gasUrl);

        //TextFieldで自動生成するScriptableObjectの名前を入力
        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("自動生成するScriptableObjectの名前");
        _scriptableObjectName = EditorGUILayout.TextField("scriptableObjectName", _scriptableObjectName);

        //ほぞんさきのPathを指定
        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("ほぞんさきのPath");
        _dataSaveFilePath = EditorGUILayout.TextField("dataSaveFilePath", _dataSaveFilePath);

        //生成するDataを選択
        EditorGUILayout.Space(_spaceSize);
        EditorGUILayout.LabelField("生成するDataのタイプを選択");
        _dataType = (DataType)EditorGUILayout.EnumPopup("GenerateDataType", _dataType);


        //生成したScriptableObjectの保存先のPath
        _scriptableObjectFilePath = "Assets/Resources/" + _dataSaveFilePath + "/" + _scriptableObjectName + " .asset";

        // GASから取得したCSVデータを保存するファイル名のパスを設定
        _outPutCsvFilePath = Application.dataPath + "/Resources/" + _dataSaveFilePath + "/" + _scriptableObjectName + "CSV" + ".csv";

        EditorGUILayout.Space(_spaceSize);
        if (GUILayout.Button("GenerateScriptableObject") && NullCheckData())
        {
            await GenerateScriptableObject();
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
        }
    }

    /// <summary> キャラクターのスクリプタブルオブジェクトを生成 </summary>
    /// <param name="parseCsvData"> 2次元配列に格納されたキャラクターのCSVデータ </param>
    private void GenerateCharacterData(string[,] parseCsvData)
    {
        CharacterDataHolder characterDataList = CreateInstance<CharacterDataHolder>();

        for (int columnCount = 2; columnCount < parseCsvData.GetLength(0); columnCount++)
        {
            PureCharacterData characterData = new();

            characterData.InitData(
                uint.Parse(parseCsvData[columnCount, 0]),
                parseCsvData[columnCount, 1],
                uint.Parse(parseCsvData[columnCount, 2]),
                uint.Parse(parseCsvData[columnCount, 3]),
                uint.Parse(parseCsvData[columnCount, 4]),
                uint.Parse(parseCsvData[columnCount, 5]),
                parseCsvData[columnCount, 6]
                );

            characterDataList.AddData(characterData);
        }

        AssetDataCreate(characterDataList);
    }

    /// <summary> サポートカードのスクリプタブルオブジェクトを生成 </summary>
    /// <param name="parseCsvData"> 2次元配列に格納されたサポートカードのCSVデータ </param>
    private void GenerateSupportCardData(string[,] parseCsvData)
    {
        SupportCardDataHolder supportCardDataHolder = CreateInstance<SupportCardDataHolder>();

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

            supportCardDataHolder.AddData(cardData);
        }

        AssetDataCreate(supportCardDataHolder);
    }

    private void AssetDataCreate(UnityEngine.Object data)
    {
        // アセットとして保存
        AssetDatabase.CreateAsset(data, _scriptableObjectFilePath);
        AssetDatabase.SaveAssets();

        //AssetDataBaseの内容を更新
        AssetDatabase.Refresh();
    }

    public enum DataType
    {
        None,
        CharacterData,
        SupportCard,
        EnemyData
    }
}
#endif