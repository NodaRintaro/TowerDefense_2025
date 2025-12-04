using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingDataHolder
{
    private DataType _dataType;

    private string _dataName;

    /// <summary> Dataの所有状況を </summary>
    private Dictionary<uint, bool> _gettingDict = new();

    public DataType DataType => _dataType;
    public string DataName => _dataName;
    public Dictionary<uint, bool> GettingDict => _gettingDict;

    public GettingDataHolder(DataType dataType, string name)
    {
        _dataType = dataType;
        _dataName = name;
    }

    public bool IsGettingData(uint characterId)
    {
        return _gettingDict[characterId];
    }

    public void DataGetting(uint id)
    {
        _gettingDict[id] = true;
    }

    public void SetDataName(string name) => _dataName = name;
    
    public void SetDataType(DataType dataType) => _dataType = dataType;
}
