using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIRoute
{
    public enum TagType
    {
        Start,
        Goal,
        Normal,
    }
    public Vector3 position;
    public TagType tag = TagType.Normal;
}

[CreateAssetMenu(fileName = "AIRoutes", menuName = "MyGame/WayPoints", order = 1)]
public class AIRoutes : ScriptableObject
{
    [field:SerializeField]
    public List<AIRoute> Points { get; private set; } = new List<AIRoute>();
}
