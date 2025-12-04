using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

// エディタウィンドウ
public class GettingDebugDataCreateEditorWindow : EditorWindow
{
    private List<GettingDataHolder> _dataHolders = new List<GettingDataHolder>();
    private Vector2 _scrollPosition;
    private int _selectedHolderIndex = -1;

    // 新規データホルダー作成用
    private string _newDataName = "NewData";
    private DataType _newDataType = DataType.None;

    // Dictionary編集用の一時変数
    private uint _newCharacterId = 0;
    private bool _newGettingStatus = false;

    // 削除対象のキー
    private List<uint> _keysToRemove = new List<uint>();

    // 折りたたみ状態
    private bool _showGettingDict = true;

    private string _saveDataName = string.Empty;

    [MenuItem("Tools/GettingDebugDataCreateEditorWindow")]
    public static void ShowWindow()
    {
        GettingDebugDataCreateEditorWindow window = GetWindow<GettingDebugDataCreateEditorWindow>("GettingDataHolder Editor");
        window.minSize = new Vector2(700, 400);
    }

    private void OnEnable()
    {
        // ウィンドウが開かれたときに自動ロード
        LoadData();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        EditorGUILayout.LabelField("GettingDataHolder Editor", EditorStyles.boldLabel);

        GUILayout.FlexibleSpace();

        // Save/Load ボタン
        if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(60)))
        {
            SaveData();
        }

        if (GUILayout.Button("Load", EditorStyles.toolbarButton, GUILayout.Width(60)))
        {
            LoadData();
        }

        if (GUILayout.Button("Open Folder", EditorStyles.toolbarButton, GUILayout.Width(90)))
        {
            OpenSaveFolder();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5);

        // データホルダー作成セクション
        DrawDataHolderCreationSection();

        EditorGUILayout.Space(10);

        // データホルダーリストとエディタ
        EditorGUILayout.BeginHorizontal();

        // 左側：データホルダーリスト
        DrawDataHolderList();

        // 右側：選択されたデータホルダーの編集エリア
        DrawDataHolderEditor();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawDataHolderCreationSection()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Create New GettingDataHolder", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Data Type:", GUILayout.Width(80));
        _newDataType = (DataType)EditorGUILayout.EnumPopup(_newDataType, GUILayout.Width(120));

        EditorGUILayout.LabelField("Data Name:", GUILayout.Width(80));
        _newDataName = EditorGUILayout.TextField(_newDataName);

        EditorGUILayout.LabelField("Save Name", GUILayout.Width(80));
        _saveDataName = EditorGUILayout.TextField(_saveDataName);

        if (GUILayout.Button("Create", GUILayout.Width(100)))
        {
            if (!string.IsNullOrEmpty(_newDataName))
            {
                _dataHolders.Add(new GettingDataHolder(_newDataType, _newDataName));
                _selectedHolderIndex = _dataHolders.Count - 1;
                _newDataName = "NewData";
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void DrawDataHolderList()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(250));

        EditorGUILayout.LabelField("Data Holders", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        for (int i = 0; i < _dataHolders.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            bool isSelected = i == _selectedHolderIndex;
            string displayName = $"[{_dataHolders[i].DataType}] {_dataHolders[i].DataName}";

            if (GUILayout.Toggle(isSelected, displayName, EditorStyles.toolbarButton))
            {
                _selectedHolderIndex = i;
            }

            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("Delete Data Holder",
                    $"Are you sure you want to delete '{_dataHolders[i].DataName}'?", "Yes", "No"))
                {
                    _dataHolders.RemoveAt(i);
                    if (_selectedHolderIndex >= _dataHolders.Count)
                    {
                        _selectedHolderIndex = _dataHolders.Count - 1;
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        if (_dataHolders.Count == 0)
        {
            EditorGUILayout.LabelField("No data holders", EditorStyles.centeredGreyMiniLabel);
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
    }

    private void DrawDataHolderEditor()
    {
        EditorGUILayout.BeginVertical();

        if (_selectedHolderIndex >= 0 && _selectedHolderIndex < _dataHolders.Count)
        {
            GettingDataHolder holder = _dataHolders[_selectedHolderIndex];

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            EditorGUILayout.LabelField("Edit Data Holder", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            // 基本情報編集
            DrawBasicInfo(holder);

            EditorGUILayout.Space(15);

            // Dictionary編集
            DrawGettingDictionary(holder);

            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.LabelField("Select or create a data holder to edit",
                EditorStyles.centeredGreyMiniLabel, GUILayout.ExpandHeight(true));
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawBasicInfo(GettingDataHolder holder)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Basic Information", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Data Type:", GUILayout.Width(100));
        DataType newType = (DataType)EditorGUILayout.EnumPopup(holder.DataType);
        if (newType != holder.DataType)
        {
            holder.SetDataType(newType);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Data Name:", GUILayout.Width(100));
        string newName = EditorGUILayout.TextField(holder.DataName);
        if (newName != holder.DataName)
        {
            holder.SetDataName(newName);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    private void DrawGettingDictionary(GettingDataHolder holder)
    {
        _showGettingDict = EditorGUILayout.Foldout(_showGettingDict,
            $"Getting Dictionary (Count: {holder.GettingDict.Count})", true);

        if (_showGettingDict)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Character ID - Getting Status", EditorStyles.miniLabel);

            _keysToRemove.Clear();

            // 既存のエントリを表示
            foreach (var kvp in holder.GettingDict)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Character ID:", GUILayout.Width(100));
                uint key = (uint)EditorGUILayout.IntField((int)kvp.Key, GUILayout.Width(100));

                EditorGUILayout.LabelField("Getting:", GUILayout.Width(60));
                bool value = EditorGUILayout.Toggle(kvp.Value, GUILayout.Width(30));

                // 状態表示
                string statusText = value ? "✓ Obtained" : "✗ Not Obtained";
                Color originalColor = GUI.color;
                GUI.color = value ? Color.green : Color.red;
                EditorGUILayout.LabelField(statusText, GUILayout.Width(100));
                GUI.color = originalColor;

                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    _keysToRemove.Add(kvp.Key);
                }

                EditorGUILayout.EndHorizontal();

                // キーまたは値が変更された場合
                if (key != kvp.Key || value != kvp.Value)
                {
                    holder.GettingDict.Remove(kvp.Key);
                    if (!holder.GettingDict.ContainsKey(key))
                    {
                        holder.GettingDict[key] = value;
                    }
                }
            }

            // 削除処理
            foreach (uint key in _keysToRemove)
            {
                holder.GettingDict.Remove(key);
            }

            if (holder.GettingDict.Count == 0)
            {
                EditorGUILayout.LabelField("No entries", EditorStyles.centeredGreyMiniLabel);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);

            // 新規追加UI
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Add New Entry", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Character ID:", GUILayout.Width(100));
            _newCharacterId = (uint)EditorGUILayout.IntField((int)_newCharacterId, GUILayout.Width(100));

            EditorGUILayout.LabelField("Getting Status:", GUILayout.Width(100));
            _newGettingStatus = EditorGUILayout.Toggle(_newGettingStatus, GUILayout.Width(30));

            if (GUILayout.Button("Add", GUILayout.Width(80)))
            {
                if (!holder.GettingDict.ContainsKey(_newCharacterId))
                {
                    holder.GettingDict[_newCharacterId] = _newGettingStatus;
                    _newCharacterId = 0;
                    _newGettingStatus = false;
                }
                else
                {
                    EditorUtility.DisplayDialog("Error",
                        $"Character ID {_newCharacterId} already exists!", "OK");
                }
            }

            EditorGUILayout.EndHorizontal();

            // クイックアクション
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Quick Add & Set True", GUILayout.Height(30)))
            {
                if (!holder.GettingDict.ContainsKey(_newCharacterId))
                {
                    holder.DataGetting(_newCharacterId);
                    _newCharacterId++;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel--;
        }
    }

    // セーブ/ロード機能
    private void SaveData()
    {
        try
        {
            DataSaveSystem.DataSave(_dataHolders, _saveDataName);
            EditorUtility.DisplayDialog("Save Successful",
                $"Data saved successfully!\nFile: {_saveDataName}.json\nLocation: {Application.persistentDataPath}", "OK");
            Debug.Log($"Data saved to: {Application.persistentDataPath}/{_saveDataName}.json");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Save Failed",
                $"Failed to save data:\n{e.Message}", "OK");
            Debug.LogError($"Save failed: {e}");
        }
    }

    private void LoadData()
    {
        try
        {
            List<GettingDataHolder> loadedData = DataSaveSystem.DataLoad<List<GettingDataHolder>>(_saveDataName);

            if (loadedData != null)
            {
                _dataHolders = loadedData;
                _selectedHolderIndex = -1;
                Debug.Log($"Data loaded from: {Application.persistentDataPath}/{_saveDataName}.json");
                Debug.Log($"Loaded {_dataHolders.Count} data holders");
            }
            else
            {
                Debug.Log("No saved data found. Starting with empty list.");
            }
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Load Failed",
                $"Failed to load data:\n{e.Message}", "OK");
            Debug.LogError($"Load failed: {e}");
        }
    }

    private void OpenSaveFolder()
    {
        string path = Application.persistentDataPath;
        EditorUtility.RevealInFinder(path);
    }
}