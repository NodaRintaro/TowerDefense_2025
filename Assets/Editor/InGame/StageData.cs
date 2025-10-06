using System;
using UnityEngine;

public class StageData : ScriptableObject
{
    public string stageName;
    public int width;
    public int height;
    public CellData[] cellDatas;

    #region 
    public StageData()
    {
        this.stageName = "New Stage";
        this.width = 5;
        this.height = 5;
        this.cellDatas = new CellData[5 * 5];
    }
    public StageData(string stageName, int  width, int height)
    {
        this.stageName = stageName;
        this.width = width;
        this.height = height;
        this.cellDatas = new CellData[width * height];
    }

    #endregion
}

[Serializable]
public class CellData
{
    public Vector3 position;
    public CellType cellType;
    public Material material;
    
    public CellData(Vector3 position, Material material)
    {
        this.position = position;
        this.material = material;
    }
}
[System.Serializable]
public enum CellType
{
    Flat,
    High,
}
