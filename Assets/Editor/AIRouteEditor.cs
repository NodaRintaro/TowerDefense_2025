using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIRoutes))]
public class AIRouteEditor : Editor
{
    private static AIRoutes instance = null;
    
    // 選択されたとき
    private void OnEnable()
    {
        instance = target as AIRoutes;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    // 選択が解除されたとき
    private void OnDisable()
    {
        instance = null;
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    // SceneViewのGUIを描画する
    private static void OnSceneGUI(SceneView sceneView)
    {
        if (instance == null) return;

        // WayPointの位置をSceneView上で変更できるハンドルを表示する
        for (int i = 0; i < instance.Points.Count; i++)
        {
            var wayPoint = instance.Points[i];
            if (i > 1)
            {
                var wayPoint2 = instance.Points[i - 1];
                Debug.DrawLine(wayPoint.position, wayPoint2.position);
            }
            // WayPointの位置を取得する
            Vector3 pos = wayPoint.position;

            // ハンドルを表示する
            EditorGUI.BeginChangeCheck();
            pos = Handles.PositionHandle(pos, Quaternion.identity);

            // WayPointの位置が変更されたら反映する
            if (EditorGUI.EndChangeCheck())
            {
                wayPoint.position = pos;
                EditorUtility.SetDirty(instance);
            }
            Handles.BeginGUI();
            // コンボボックス
            // スクリーン座標に変換する
            var screenPos = HandleUtility.WorldToGUIPointWithDepth(pos);
            // コンボボックスを表示する
            EditorGUI.BeginChangeCheck();
            var rect = new Rect(screenPos.x, screenPos.y + 10, 100, 20);
            var editedTag = (AIRoute.TagType)EditorGUI.EnumPopup(rect, wayPoint.tag);
            // 変更されたら反映する
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(instance, "Edit Destination");
                wayPoint.tag = editedTag;
                EditorUtility.SetDirty(instance);
            }

            Handles.EndGUI();
        }
    }
}
