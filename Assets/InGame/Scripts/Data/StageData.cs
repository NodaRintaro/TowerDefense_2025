using System;
using UnityEngine;
[Serializable]
public class StageData : ScriptableObject
{
    public string stageName;
    public int towerHealth;
    public int initialCoinNum = 30;// コインの初期値
    public float generateCoinSpeed; // 一秒間にコインを生成する数
    public int width;
    public int height;
    public CellData[] cellDatas;
    public WaveData[] waveDatas;

    #region 
    public StageData()
    {
        this.stageName = "New Stage";
        this.towerHealth = 10;
        this.initialCoinNum = 30;
        this.generateCoinSpeed = 1f;
        this.width = 5;
        this.height = 5;
        this.cellDatas = new CellData[5 * 5];
        this.waveDatas = new WaveData[0];
    }
    public StageData(string stageName, int towerHealth, int  width, int height, CellData[] cellDatas)
    {
        this.stageName = stageName;
        this.towerHealth = towerHealth;
        this.width = width;
        this.height = height;
        this.cellDatas = cellDatas;
        this.waveDatas = new WaveData[0];
    }
    #endregion
}

[Serializable]
public class CellData
{
    public CellType cellType;
    public Material material;
    public CellData() { this.cellType = CellType.Flat; }
    public CellData(Material material) { this.material = material; }
    public CellData(CellType cellType) {this.cellType = cellType;}
}
[System.Serializable]
public enum CellType
{
    Flat,
    High,
}
