using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "AIRoutes", menuName = "InGame/WayPoints", order = 1)]
public class AIRoutes : ScriptableObject
{
    [field: SerializeField]
    public List<Vector3> Points
    {
        get;
        private set;
    } = new List<Vector3>();
    public int Count => Points.Count;
}
