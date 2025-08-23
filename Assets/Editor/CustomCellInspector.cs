using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor( typeof( CellGridMove ) )]
public sealed class CustomCellInspector : Editor
{
    private const float SPACE       = 1;
    private const float SPACE_HALF  = SPACE / 2;
    private static Transform _stageTransform;

    void Awake()
    {
        //Unity起動時にStageObjectを検索する。
        
    }

    // private void OnEnable()
    // {
    //     //Unity開始時、また新たにオブジェクトを配置するときにStageObjectの子オブジェクトにする。
    //     if (PrefabStageUtility.GetCurrentPrefabStage() == null)
    //     {
    //         var example = target as CellGridMove;
    //         SetParent(example.transform);
    //     }
    // }

    private void OnSceneGUI()
    {
        var example = target as CellGridMove;
        var posX    = serializedObject.FindProperty( "m_posX"   );
        var posY    = serializedObject.FindProperty( "m_posY"   );
        var posZ    = serializedObject.FindProperty( "m_posZ"   );
        var scaleX  = serializedObject.FindProperty( "m_scaleX" );
        var scaleY  = serializedObject.FindProperty( "m_scaleY" );
        var scaleZ  = serializedObject.FindProperty( "m_scaleZ" );

        var transform = example.transform;

        {
            var position = new Vector3( posX.intValue, posY.intValue, posZ.intValue );
            var result = Handles.PositionHandle( position, transform.rotation );
            result.x = MultipleRound( result.x, SPACE_HALF );
            result.y = MultipleRound( result.y, SPACE_HALF );
            result.z = MultipleRound( result.z, SPACE_HALF );
            posX.intValue = ( int )result.x;
            posY.intValue = ( int )result.y;
            posZ.intValue = ( int )result.z;
            transform.position = result;
        }
        serializedObject.ApplyModifiedProperties();
    }

    private static float MultipleFloor( float value, float multiple )
    {
        return Mathf.Floor( value / multiple ) * multiple;
    }

    private static float MultipleRound( float value, float multiple )
    {
        return MultipleFloor( value + multiple * 0.5f, multiple );
    }
    
    // private static void SetParent(Transform transform)
    // {
    //     if (_stageTransform == null)
    //     {
    //         //Debug.LogError("StageObjIsNull");
    //         var stageObj = GameObject.Find( "Stage" );
    //         if ( stageObj == null )
    //         {
    //             stageObj = new GameObject( "Stage" );
    //         }
    //         _stageTransform = stageObj.transform;
    //     }
    //     transform.SetParent( _stageTransform.transform );
    // }
}