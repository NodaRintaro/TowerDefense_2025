using System;
using UnityEngine;

public class StageData : ScriptableObject
{
    private string _stageName;
    private CellData[] _cellDatas;

    public void SetCells(CellData[] cellDatas) { _cellDatas = cellDatas;}
}

[Serializable]
public class CellData
{
    public Vector3 position;
    public Material material;
}
