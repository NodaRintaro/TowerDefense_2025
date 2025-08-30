using System;
using UnityEngine;

public class StageData : ScriptableObject
{
    private string _stageName;
    private CellData[] _cellDatas;

    public void SetCells(CellData[] cellDatas) { _cellDatas = cellDatas;}
    public void SetCell(int index, CellData cellData) { _cellDatas[index] = cellData; }
    
    public CellData[] GetCells() { return _cellDatas; }
    public CellData GetCell(int index) { return _cellDatas[index]; }
}

[Serializable]
public class CellData
{
    public Vector3 position;
    public Material material;
    
    public CellData(Vector3 position, Material material)
    {
        this.position = position;
        this.material = material;
    }
}
