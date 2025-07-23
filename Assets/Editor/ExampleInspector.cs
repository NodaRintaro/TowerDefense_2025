using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( Example ) )]
public sealed class ExampleInspector : Editor
{
    private const float SPACE       = 1;
    private const float SPACE_HALF  = SPACE / 2;

    private void OnSceneGUI()
    {
        var example = target as Example;
        var posX    = serializedObject.FindProperty( "m_posX"   );
        var posZ    = serializedObject.FindProperty( "m_posZ"   );
        var scaleX  = serializedObject.FindProperty( "m_scaleX" );
        var scaleZ  = serializedObject.FindProperty( "m_scaleZ" );

        var transform = example.transform;

        {
            var position = new Vector3( posX.intValue, 0, posZ.intValue );
            var result = Handles.PositionHandle( position, transform.rotation );
            result.x = MultipleRound( result.x, SPACE_HALF );
            result.z = MultipleRound( result.z, SPACE_HALF );
            posX.intValue = ( int )result.x;
            posZ.intValue = ( int )result.z;
            transform.position = result;
        }

        {
            // var scale = transform.localScale;//new Vector3( scaleX.intValue, 0, scaleZ.intValue );
            // var size = 1;//HandleUtility.GetHandleSize( transform.position ) * 1.5f;
            // var result  = Handles.ScaleHandle
            // (
            //     scale       : scale,
            //     position    : transform.position,
            //     rotation    : transform.rotation,
            //     size        : size
            // );
            // result.x = MultipleRound( result.x, SPACE );
            // result.z = MultipleRound( result.z, SPACE );
            // scaleX.intValue = ( int )result.x;
            // scaleZ.intValue = ( int )result.z;
            // transform.localScale = result;
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
}