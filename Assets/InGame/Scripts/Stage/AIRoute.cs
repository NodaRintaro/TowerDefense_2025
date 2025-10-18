using System;
using UnityEngine;

[Serializable]
// [CreateAssetMenu(fileName = "AIRoute", menuName = "InGame/WayPoints", order = 1)]
public class AIRoute// : ScriptableObject
{
    [field: SerializeField]
    // public List<Vector3> Points
    // {
    //     get;
    //     private set;
    // } = new List<Vector3>();
    public Vector3[] points = new Vector3[]{};
    public int Length => points.Length;
    public void AddPoint(Vector3 point)
    {
        Array.Resize(ref points, points.Length + 1);
    }
    public void RemoveAtPoint(int index)
    {
        if (points.Length <= 0 || index < 0 || index >= points.Length) return;
        Vector3[] vecs = new Vector3[points.Length - 1];
        for (int i = 0, j = 0; i < vecs.Length; i++, j++)
        {
            if (j == index) j++;
            vecs[i] = points[j];
        }
    }
}
