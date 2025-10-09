using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateStageWindow : EditorWindow
{
    private CellData[] _cellData;
    
    private Vector2 _scrollPos;
    private int _scrollHeight = 600;
    
    private int _width = 5;
    private int _height = 5;
    
    private int _spaceSize = 20;
    private int _displayGridSize = 25;
    
    //Gridの表示サイズの最低値と最高値
    private int _minDisplayGridSize = 1;
    private int _maxDisplayGridSize = 50;

    private const string MustSavePath = "Assets/Resources/StageData/";
	private string _saveFolderPath = "DefaultStages";
    private string _stageName = "NewStage";
    
    private bool _isInit = false;
    
    [MenuItem("Tools/CreateStageWindow")]
    public static void ShowWindow()
    {
        GetWindow<CreateStageWindow>();
    }
    
    private void OnGUI()
    {
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(_scrollHeight));
        
        //初期化前に行うべき設定
        if (!_isInit)
        {
            EditorGUILayout.LabelField("作る部屋の大きさを設定");
            _width = EditorGUILayout.IntField("横の大きさ:", _width);
            _height = EditorGUILayout.IntField("縦の大きさ:", _height);
            EditorGUILayout.LabelField("保存先のPathを指定");
            _saveFolderPath　= EditorGUILayout.TextField(_saveFolderPath);
        }
        
        //1行空ける
        EditorGUILayout.Space(_spaceSize);
        
        //ボタンを押すと初期化されるようにする
        if (GUILayout.Button("Initialize Grid"))
        {
            InitGrid();
        }

        //ボタンを押すと1つ前の画面に戻って部屋の大きさを設定できるようになる
        if (GUILayout.Button("Delete Grid"))
        {
            _cellData = null;
            _isInit = false;
        }
        
        EditorGUILayout.Space(_spaceSize);
        
        if(_isInit)
        {
            StageMakeGUI();
        }
        
        EditorGUILayout.EndScrollView();
    }

    #region Gridの表示処理
    /// <summary>Gridの表示</summary>
    private void StageMakeGUI()
    {
        EditorGUILayout.LabelField("新しく作るステージの名前");
        _stageName = EditorGUILayout.DelayedTextField(_stageName);
        EditorGUILayout.LabelField("現在のステージの大きさ");
        EditorGUILayout.LabelField("横の長さ:" + _width + "マス");
        EditorGUILayout.LabelField("縦の長さ:" + _height + "マス");
        
        EditorGUILayout.Space();
        
        for (int y = 0; y < _height; y++)
        {
            //配列の要素数を横に表示する
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < _width; x++)
            {
                CellType currentTile = GetCell(x, y);

                Color color = GetColor(currentTile);
                GUI.backgroundColor = color;

                if (GUILayout.Button(currentTile.ToString().Substring(0, 1), GUILayout.Width(_displayGridSize), GUILayout.Height(_displayGridSize)))
                {
                    CellType nextType = ChangeCellType(currentTile);
                    SetCell(x, y, nextType);
                }

                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndHorizontal();
        }
        
        if (GUILayout.Button("Save StageGrid"))
        {
            SaveStageData();
        }
        
        _displayGridSize = EditorGUILayout.IntSlider("セルの表示サイズ", _displayGridSize, _minDisplayGridSize, _maxDisplayGridSize);
    }
    #endregion

    #region Gridの初期化
    /// <summary>Gridの初期化</summary>
    private void InitGrid()
    {
        _cellData = new CellData[_width * _height];
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _cellData[i + _width * j] = new CellData();
                _cellData[i + _width * j].cellType = CellType.Flat;
            }
        }

        _isInit = true;
    }
    #endregion

    #region Dataの保存
    /// <summary>部屋のデータを保存</summary>
    private void SaveStageData()
    {
        if (!Directory.Exists(MustSavePath + _saveFolderPath))
        {
            Directory.CreateDirectory(MustSavePath + _saveFolderPath);
        }
        StageData stageData = ScriptableObject.CreateInstance<StageData>();
        
        //Dataを保存
        stageData.stageName = _stageName;
        stageData.width = _width;
        stageData.height = _height;
        stageData.cellDatas =  _cellData;
        
        // 保存パスの指定
        string assetPath = $"{MustSavePath + _saveFolderPath}/StageData_{_stageName + System.DateTime.Now.Ticks}.asset";
        
        AssetDatabase.CreateAsset(stageData, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        // 作成後に選択状態に
        Selection.activeObject = stageData;
    }
    #endregion
	
    /// <summary>clickされた際にCellTypeを変える機能</summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private CellType ChangeCellType(CellType type)
    {
        int next = ((int)type + 1) % System.Enum.GetValues(typeof(CellType)).Length;
        return (CellType)next;
    }

    /// <summary>グリッド上の表記をわかり安くする</summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Color GetColor(CellType type)
    {
        switch (type)
        {
            case CellType.Flat: return Color.green;
            case CellType.High: return Color.black;
            default: return Color.magenta;
        }
    }
    
    /// <summary>指定したセルの情報を取得</summary>
    public CellType GetCell(int x, int y)
    {
        return _cellData[CellPos(x,y)].cellType;
    }
    
    /// <summary>指定したセルのタイプを変更</summary>
    public void SetCell(int tilePosX, int tilePosY, CellType cellType)
    {
        _cellData[CellPos(tilePosX,tilePosY)].cellType = cellType;
    }

    /// <summary>指定した座標のセルの位置を計算</summary>
    private int CellPos(int x, int y)
    {
        return x + y * _width;
    }
}
