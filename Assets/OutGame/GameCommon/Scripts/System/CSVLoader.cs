using UnityEngine;

public class CSVLoader
{
    public static string[,] LoadCsv(TextAsset target)
    {
        // 1. 改行で分割して行を取得
        // \r\n (Windows), \n (Unix) 両対応のため Split のオプションを指定
        string[] lines = target.text.Split(new[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

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
                    result[i, j] = tempColumns[j].Trim();
                }
            }
        }
        return result;
    }
}