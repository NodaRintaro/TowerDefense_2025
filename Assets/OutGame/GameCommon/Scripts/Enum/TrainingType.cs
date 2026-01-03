using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrainingType 
{
    /// <summary> トレーニング内容が設定されていません </summary>
    [InspectorName("トレーニング内容が設定されていません")]
    None,
    /// <summary> マラソン </summary>
    [InspectorName("マラソン")]
    Physical,
    /// <summary> 筋トレ </summary>
    [InspectorName("筋トレ")]
    Power,
    /// <summary> 読書 </summary>
    [InspectorName("読書")]
    Intelligence,
    /// <summary> 狩猟 </summary>
    [InspectorName("狩猟")]
    Speed
}
