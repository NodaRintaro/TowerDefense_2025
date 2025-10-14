using System;
using UnityEngine;
[Serializable]
public class StageData : ScriptableObject
{
    public string stageName;
    public int width;
    public int height;
    public CellData[] cellDatas;
    public WaveData[] waveDatas;

    #region 
    public StageData()
    {
        this.stageName = "New Stage";
        this.width = 5;
        this.height = 5;
        this.cellDatas = new CellData[5 * 5];
    }
    public StageData(string stageName, int  width, int height, CellData[] cellDatas)
    {
        this.stageName = stageName;
        this.width = width;
        this.height = height;
        this.cellDatas = cellDatas;
    }

    #endregion
}

[Serializable]
public class CellData
{
    public CellType cellType;
    public Material material;
    public CellData()
    {
        this.cellType = CellType.Flat;
    }
    
    public CellData(Material material) { this.material = material; }
    public CellData(CellType cellType) {this.cellType = cellType;}
}
[System.Serializable]
public enum CellType
{
    Flat,
    High,
}
