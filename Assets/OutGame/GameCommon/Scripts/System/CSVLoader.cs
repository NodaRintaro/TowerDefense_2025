using UnityEngine;

public class CSVLoader
{
    public static string[,] LoadCsv(TextAsset target)
    {
        if (target == null || string.IsNullOrEmpty(target.text))
        {
            Debug.LogError("CSV data is null or empty.");
            return new string[0, 0]; 
        }
        // 1. 改行で分割して行を取得
        // \r\n (Windows), \n (Unix) 両対応のため Split のオプションを指定
        string[] lines = target.text.Split(new[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length == 0)
        {
            Debug.LogError("No data rows found in CSV.");
            return new string[0, 0]; 
        }
        
        int rowCount = lines.Length;
        int colCount = lines[0].Split(',').Length;

        string[,] result = new string[rowCount, colCount];

        for (int i = 0; i < rowCount; i++)
        {
            string[] tempColumns = lines[i].Split(',');
            for (int j = 0; j < colCount; j++)
            {
                // 列数が異なる行がある場合の境界チェック
                if (j < tempColumns.Length)
                {
                    string cleanedValue = tempColumns[j].Trim();
                    // 前後の二重引用符を除去
                    if (cleanedValue.StartsWith("\"") && cleanedValue.EndsWith("\"") && cleanedValue.Length > 1)
                    {
                        cleanedValue = cleanedValue.Substring(1, cleanedValue.Length - 2);
                    }
                    result[i, j] = cleanedValue;
                }
            }
        }
        Debug.Log("データの分割に成功");
        return result;
    }
}