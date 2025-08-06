using System;
using UnityEngine;
[Serializable]
public sealed class GridMove : MonoBehaviour
{
    [SerializeField][HideInInspector] private int m_posX    = 0;
    [SerializeField][HideInInspector] private int m_posZ    = 0;
    [SerializeField][HideInInspector] private int m_scaleX  = 0;
    [SerializeField][HideInInspector] private int m_scaleZ  = 0;
}