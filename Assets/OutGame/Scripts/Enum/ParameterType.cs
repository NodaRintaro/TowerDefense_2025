using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParameterType
{
    [InspectorName("筋力")]
    Power,
    [InspectorName("体力")]
    Physical,
    [InspectorName("知力")]
    Intelligence,
    [InspectorName("素早さ")]
    Speed,
}
