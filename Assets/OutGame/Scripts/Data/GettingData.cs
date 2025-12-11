using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GettingData
{
    public class GettingDataHolder
    {
        private DataType _dataType;

        /// <summary> Dataの所有状況を </summary>
        private Dictionary<uint, bool> _gettingDict = new();

        public DataType DataType => _dataType;
        public Dictionary<uint, bool> GettingDict => _gettingDict;

        public GettingDataHolder(DataType dataType)
        {
            _dataType = dataType;
        }

        public bool IsGettingData(uint characterId)
        {
            return _gettingDict[characterId];
        }

        public void DataGetting(uint id)
        {
            _gettingDict[id] = true;
        }

        public void SetDataType(DataType dataType) => _dataType = dataType;
    }

}