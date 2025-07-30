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
    
    [MenuItem("Tools/CreateStage")]
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
        
        //Stageというオブジェクトの子オブジェクトを取得する。
        GameObject stageObj = GameObject.Find("Stage");
        int childObjCount = stageObj.transform.childCount;
        CellData[] cellDatas = new CellData[childObjCount];
        for (int i = 0; i < childObjCount; i++)
        {
            cellDatas[i].position = stageObj.transform.GetChild(i).position;
        }

        //ステージデータを作る
        StageData stageData = ScriptableObject.CreateInstance<StageData>();
        stageData.SetCells(cellDatas);
        
        // 保存パスの指定
        string assetPath = $"{_mustSavePath + _saveFolderPath}/RoomData_{_stageName + System.DateTime.Now.Ticks}.asset";
        
        AssetDatabase.CreateAsset(stageData, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
