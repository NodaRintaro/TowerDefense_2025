using System;
using UnityEngine;

[Serializable]
public class AIRoute// : ScriptableObject
{
    [field: SerializeField]
    public Vector3[] points = new Vector3[]{};
    public int Length => points.Length;
    public void AddPoint(Vector3 point)
    {
        Array.Resize(ref points, points.Length + 1);
    }
    public void RemoveAtPoint(int index)
    {
        if (points.Length <= 0 || index < 0 || index > points.Length) return;
        Debug.Log("RemovePoint");
        Vector3[] vecs = new Vector3[points.Length - 1];
        for (int i = 0, j = 0; i < vecs.Length; i++, j++)
        {
            if (j == index) j++;
            vecs[i] = points[j];
        }

        points = vecs;
    }
}
