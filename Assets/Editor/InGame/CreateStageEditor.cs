using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageData))]
public class CreateStageEditor : Editor
{
    //スクロールしている場所の位置
    private Vector2 _scrollPos;
    private int _scrollHeight = 500;
    
    //空白の大きさ
    private int _spaceSize = 10;
    
    //表示するグリッドの大きさ
    private int _displayGridSize = 25;
    
    //Gridの表示サイズの最低値と最高値
    private int _minDisplayGridSize = 1;
    private int _maxDisplayGridSize = 50;
    
    public override void OnInspectorGUI()
    {
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(_scrollHeight));
        
        StageData stageData = (StageData)target;
        
        //nullの場合Dataを初期化
        if (stageData.cellDatas == null)
        {
            stageData.cellDatas = new CellData[stageData.cellDatas.Length];
            EditorUtility.SetDirty(stageData);
        }
        
        EditorGUILayout.LabelField("横のステージの長さ:" + stageData.width);
        EditorGUILayout.LabelField("縦のステージの長さ:" + stageData.height);

        EditorGUILayout.Space(_spaceSize);
        
        TileTypeGUI(stageData, _displayGridSize);
        
        _displayGridSize = EditorGUILayout.IntSlider("ステージの表示サイズ", _displayGridSize, _minDisplayGridSize, _maxDisplayGridSize);
        
        EditorGUILayout.EndScrollView();
    }

    private void TileTypeGUI(StageData stageData, int displaySize)
    {
        CellType currentCell;
        Color tileColor;
        
        for (int y = 0; y < stageData.height; y++)
        {
            //配列の要素数を横に表示する
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < stageData.width; x++)
            { 
                currentCell = stageData.cellDatas[x + stageData.width * y].cellType;
                tileColor = GetCellColor(currentCell);
                GUI.backgroundColor = tileColor;
                GUILayout.Button(currentCell.ToString().Substring(0, 1), GUILayout.Width(_displayGridSize), GUILayout.Height(_displayGridSize));
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndHorizontal();
        }
    }
    
    /// <summary>グリッド上の表記をわかり安くする</summary>
    private Color GetCellColor(CellType type)
    {
        switch (type)
        {
            case CellType.Flat: return Color.white;
            case CellType.High: return Color.green;
            default: return Color.magenta;
        }
    }
    
    /// <summary>clickされた際にTileTypeを変える処理</summary>
    private CellType ChangeTileType(CellType type)
    {
        int next = ((int)type + 1) % System.Enum.GetValues(typeof(CellType)).Length;
        return (CellType)next;
    }
}
