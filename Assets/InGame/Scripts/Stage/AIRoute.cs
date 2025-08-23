using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIRoute
{
    public Vector3 position;
}

[CreateAssetMenu(fileName = "AIRoutes", menuName = "InGame/WayPoints", order = 1)]
public class AIRoutes : ScriptableObject
{
    [field: SerializeField]
    public List<AIRoute> Points
    {
        get;
        private set;
    } = new List<AIRoute>();
}
