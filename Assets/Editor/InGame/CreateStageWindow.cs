using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateStageWindow : EditorWindow
{
    private int _width = 5;
    private int _height = 5;
    
    private const string _mustSavePath = "Assets/Resources/StageData/";
    private string _saveFolderPath = "DefaultRooms";
    private string _stageName = "NewRoom";
    
    [MenuItem("Tools/CreateStageData")]
    public static void ShowWindow()
    {
        GetWindow<CreateStageWindow>();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("新しくステージの名前");
        _stageName = EditorGUILayout.DelayedTextField(_stageName);
        if (GUILayout.Button("Save RoomGrid"))
        {
            MakeStageData();
        }
    }

    //ステージデータをscriptableObjectとして保存する
    private void MakeStageData()
    {
        if (!Directory.Exists(_mustSavePath + _saveFolderPath))
        {
            Directory.CreateDirectory(_mustSavePath + _saveFolderPath);
        }
        
        StageData stageData = ScriptableObject.CreateInstance<StageData>();
        stageData.name = _stageName;
        
        //Stageというオブジェクトの子オブジェクトを取得する。
        GameObject stageObj = GameObject.Find("Stage");
        int childObjCount = stageObj.transform.childCount;
        stageData.SetCells(new CellData[childObjCount]);
        for (int i = 0; i < childObjCount; i++)
        {
            stageData.SetCell(i, new CellData(stageObj.transform.GetChild(i).position, default));
        }

        
        // 保存パスの指定
        string assetPath = $"{_mustSavePath + _saveFolderPath}/RoomData_{_stageName + System.DateTime.Now.Ticks}.asset";
        
        AssetDatabase.CreateAsset(stageData, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void EditStageByStageData()
    {
        EditorGUILayout.LabelField("Edit Stage by StageData");
        StageData stageData = (StageData)EditorGUILayout.ObjectField(null, typeof(StageData), false);
        if (stageData == null)
        {
            return;
        }
        
        // ステージデータを元にステージを生成する
        GameObject stageObj = GameObject.Find("Stage");
        if (stageObj == null)
        {
            stageObj = new GameObject("Stage");
        }
        //stageObj.transform.GetChild(1).
        for (int i = 0; i < stageData.GetCells().Length; i++)
        {
            GameObject cellObj = new GameObject("Cell");
            cellObj.transform.parent = stageObj.transform;
            cellObj.transform.position = stageData.GetCell(i).position;
        }
    }
}
