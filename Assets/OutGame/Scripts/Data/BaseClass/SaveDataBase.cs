using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DataSaveSystemでSave可能なDataの基底クラス
/// </summary>
public abstract class SaveDataBase : ISaveData
{
    private string _savePath = null;

    public SaveDataBase()
    {
        SetSavePath();
    }

    /// <summary> セーブデータのPathを宣言する処理 </summary>
    private void SetSavePath()
    {
        _savePath = "SaveData_" + this.GetType().Name;
    }

    /// <summary> Dataを保存する処理 </summary>
    public void DataSave()
    {
        JsonDataSaveSystem.DataSave(this, _savePath);
    }

    public void DataDelete()
    {
        JsonDataSaveSystem.DataDelete(_savePath);
    }
}
