using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageData))]
public class CreateStageEditor : Editor
{
    private static StageData _instance;
    GameObject _parent;
    GameObject[] _grid;
    Color _aiRouteColor = Color.green;
    Color _stageWireColor = Color.gray;
    
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
    
    private const float SPACE       = 1;
    private void OnEnable()
    {
        _instance = target as StageData;
        if(_instance.aiRoute == null) _instance.aiRoute = ScriptableObject.CreateInstance<AIRoute>();
        SceneView.duringSceneGui += OnSceneGUI;
        
        CreateGrid();
    }

    // 選択が解除されたとき
    private void OnDisable()
    {
        _instance = null;
        SceneView.duringSceneGui -= OnSceneGUI;

        DestroyGrid();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (_instance == null) return;
        for (int i = 0; i < _instance.width; i++)
        {
            for (int j = 0; j < _instance.height; j++)
            {
                GameObject gridObj = _grid[i + j * _instance.width];
                CellData cellData = _instance.cellDatas[i + j * _instance.width];
                Material mat = new Material( Shader.Find("Standard") );
                if (cellData.cellType == CellType.Flat)
                {
                    mat.color = Color.white;
                    gridObj.transform.position = new Vector3(i, 0, -j);
                }
                else if (cellData.cellType == CellType.High)
                {
                    mat.color = Color.green;
                    gridObj.transform.position = new Vector3(i, 0.5f, -j);
                }
                else
                {
                    mat.color = Color.magenta;
                    gridObj.transform.position = new Vector3(i, 0, -j);
                }
                gridObj.GetComponent<Renderer>().material = mat;
                
                Handles.color = _stageWireColor;
                //Handles.DrawWireCube(gridObj.transform.position, Vector3.one);
                Vector3[] vecs = new Vector3[4]
                {
                    new Vector3(0.5f,0.5f,0.5f) + gridObj.transform.position,
                    new Vector3(0.5f,0.5f,-0.5f) + gridObj.transform.position,
                    new Vector3(-0.5f,0.5f,-0.5f) + gridObj.transform.position,
                    new Vector3(-0.5f,0.5f,0.5f) + gridObj.transform.position
                };
                Handles.DrawSolidRectangleWithOutline(vecs,new Color(0,0,0,0),Color.grey);
            }
        }

        if (_instance.aiRoute == null)
        {
            Debug.Log("AIRouteが設定されていません");
            return;
        }

        #region AIRoute用GUI
        // WayPointの位置をSceneView上で変更できるハンドルを表示する
        for (int i = 0; i < _instance.aiRoute.Points.Count; i++)
        {
            // WayPointの位置を取得する
            Vector3 wayPoint = _instance.aiRoute.Points[i];
            if (i >= 1)
            {
                var wayPoint2 = _instance.aiRoute.Points[i - 1];
                Handles.color = _aiRouteColor;
                Handles.DrawLine(wayPoint + new Vector3(0, 0.5f, 0), wayPoint2 + new Vector3(0, 0.5f, 0), 2f);
            }
            // WayPointの位置を取得する
            Vector3 pos = wayPoint;

            // ハンドルを表示する
            EditorGUI.BeginChangeCheck();
            pos = Handles.PositionHandle(pos, Quaternion.identity);

            // WayPointの位置が変更されたら反映する
            if (EditorGUI.EndChangeCheck())
            {
                pos = MultipleFloor(pos, SPACE);
                _instance.aiRoute.Points[i] = pos;
                EditorUtility.SetDirty(_instance.aiRoute);
            }
            Handles.BeginGUI();
            // コンボボックス
            // スクリーン座標に変換する
            var screenPos = HandleUtility.WorldToGUIPointWithDepth(pos);
            // コンボボックスを表示する
            EditorGUI.BeginChangeCheck();
            var rect = new Rect(screenPos.x, screenPos.y + 10, 100, 20);
            EditorGUI.TextField(rect, $"{i}");
            // 変更されたら反映する
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_instance.aiRoute, "Edit Destination");
                EditorUtility.SetDirty(_instance.aiRoute);
            }

            Handles.EndGUI();
        }
        #endregion
    }
    
    public override void OnInspectorGUI()
    {
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(_scrollHeight));
        
        //nullの場合Dataを初期化
        if (_instance.cellDatas == null)
        {
            _instance.cellDatas = new CellData[_instance.cellDatas.Length];
            EditorUtility.SetDirty(_instance);
        }
        
        EditorGUILayout.LabelField("横のステージの長さ:" + _instance.width);
        EditorGUILayout.LabelField("縦のステージの長さ:" + _instance.height);

        EditorGUILayout.Space(_spaceSize);
        
        TileTypeGUI(_instance, _displayGridSize);
        
        _displayGridSize = EditorGUILayout.IntSlider("ステージの表示サイズ", _displayGridSize, _minDisplayGridSize, _maxDisplayGridSize);
        
        AIRouteGUI();
        
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
                if (GUILayout.Button(currentCell.ToString().Substring(0, 1), GUILayout.Width(_displayGridSize),
                        GUILayout.Height(_displayGridSize)))
                {
                    stageData.cellDatas[x + stageData.width * y].cellType = ChangeCellType(currentCell);
                }
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void AIRouteGUI()
    {
        if (GUILayout.Button("Add WayPoint"))
        {
            _instance.aiRoute.Points.Add(Vector3.zero);
        }

        if (GUILayout.Button("Remove WayPoint"))
        {
            if (_instance.aiRoute.Points.Count > 0)
            {
                _instance.aiRoute.Points.RemoveAt(_instance.aiRoute.Points.Count - 1);
            }
        }
    }

    private void CreateGrid()
    {
        if (_parent == null)
        {
            _parent = new GameObject("Grid");
        }
        _grid = new GameObject[_instance.width * _instance.height];
        for (int i = 0; i < _grid.Length; i++)
        {
            _grid[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _grid[i].transform.position = new Vector3(i % _instance.width, 0, i / _instance.width);
            _grid[i].transform.localScale = Vector3.one;
            _grid[i].transform.parent = _parent.transform;
        }
    }

    private void DestroyGrid()
    {
        foreach (var variable in _grid)
        {
            DestroyImmediate(variable.gameObject);
        }
        DestroyImmediate(_parent.gameObject);
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
    private CellType ChangeCellType(CellType type)
    {
        int next = ((int)type + 1) % System.Enum.GetValues(typeof(CellType)).Length;
        return (CellType)next;
    }
    private static Vector3 MultipleFloor( Vector3 value, float multiple )
    {
        Vector3 vec = new Vector3();
        vec.x = Mathf.Floor( value.x / multiple ) * multiple;
        vec.y = Mathf.Floor( value.y / multiple ) * multiple;
        vec.z = Mathf.Floor( value.z / multiple ) * multiple;
        return vec;
    }
}
