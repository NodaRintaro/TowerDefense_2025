using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
// [CreateAssetMenu(fileName = "AIRoute", menuName = "InGame/WayPoints", order = 1)]
public class AIRoute// : ScriptableObject
{
    [field: SerializeField]
    public List<Vector3> Points
    {
        get;
        private set;
    } = new List<Vector3>();
    public int Count => Points.Count;
}
