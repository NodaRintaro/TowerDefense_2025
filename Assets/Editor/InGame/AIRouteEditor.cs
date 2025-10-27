using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(AIRoute))]
public class AIRouteEditor : Editor
{
    // private static AIRoute instance = null;
    // private const float SPACE       = 1;
    //
    // // 選択されたとき
    // private void OnEnable()
    // {
    //     instance = target as AIRoute;
    //     SceneView.duringSceneGui += OnSceneGUI;
    // }
    //
    // // 選択が解除されたとき
    // private void OnDisable()
    // {
    //     instance = null;
    //     SceneView.duringSceneGui -= OnSceneGUI;
    // }
    //
    // // SceneViewのGUIを描画する
    // private static void OnSceneGUI(SceneView sceneView)
    // {
    //     if (instance == null) return;
    //
    //     // WayPointの位置をSceneView上で変更できるハンドルを表示する
    //     for (int i = 0; i < instance.Points.Count; i++)
    //     {
    //         var wayPoint = instance.Points[i];
    //         if (i > 1)
    //         {
    //             var wayPoint2 = instance.Points[i - 1];
    //             Debug.DrawLine(wayPoint, wayPoint2);
    //         }
    //         // WayPointの位置を取得する
    //         Vector3 pos = wayPoint;
    //
    //         // ハンドルを表示する
    //         EditorGUI.BeginChangeCheck();
    //         pos = Handles.PositionHandle(pos, Quaternion.identity);
    //
    //         // WayPointの位置が変更されたら反映する
    //         if (EditorGUI.EndChangeCheck())
    //         {
    //             pos = MultipleFloor(pos, SPACE);
    //             wayPoint = pos;
    //             instance.Points[i] = wayPoint;
    //             EditorUtility.SetDirty(instance);
    //         }
    //         Handles.BeginGUI();
    //         // コンボボックス
    //         // スクリーン座標に変換する
    //         var screenPos = HandleUtility.WorldToGUIPointWithDepth(pos);
    //         // コンボボックスを表示する
    //         EditorGUI.BeginChangeCheck();
    //         var rect = new Rect(screenPos.x, screenPos.y + 10, 100, 20);
    //         EditorGUI.TextField(rect, $"{i}");
    //         // 変更されたら反映する
    //         if (EditorGUI.EndChangeCheck())
    //         {
    //             Undo.RecordObject(instance, "Edit Destination");
    //             EditorUtility.SetDirty(instance);
    //         }
    //
    //         Handles.EndGUI();
    //     }
    // }
    // private static Vector3 MultipleFloor( Vector3 value, float multiple )
    // {
    //     Vector3 vec = new Vector3();
    //     vec.x = Mathf.Floor( value.x / multiple ) * multiple;
    //     vec.y = Mathf.Floor( value.y / multiple ) * multiple;
    //     vec.z = Mathf.Floor( value.z / multiple ) * multiple;
    //     return vec;
    // }
}
