using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrainingType 
{
    [InspectorName("トレーニング内容が設定されていません")]
    None,
    [InspectorName("マラソン")]
    Physical,
    [InspectorName("筋トレ")]
    Power,
    [InspectorName("読書")]
    Intelligence,
    [InspectorName("マラソン")]
    Speed
}
