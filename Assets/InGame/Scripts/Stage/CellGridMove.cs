using System;
using UnityEngine;
/// <summary>これをアタッチしたオブジェクトはグリッドに沿って動くようになる</summary>
[Serializable]
public sealed class CellGridMove : MonoBehaviour
{
    [SerializeField][HideInInspector] private int m_posX    = 0;
    [SerializeField][HideInInspector] private int m_posY    = 0;
    [SerializeField][HideInInspector] private int m_posZ    = 0;
    [SerializeField][HideInInspector] private int m_scaleX  = 0;
    [SerializeField][HideInInspector] private int m_scaleY  = 0;
    [SerializeField][HideInInspector] private int m_scaleZ  = 0;
}