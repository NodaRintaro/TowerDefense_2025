using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerDefenseCharacterDataBase : IJsonSaveData
{
    /// <summary> key = BaseCharacterID, Value = 同じベースキャラクターの配列 </summary>
    private Dictionary<uint, TowerDefenseCharacterData[]> _towerDefenseCharacterDataDict = new();

    private const uint _maxCharacterNum = 20;

    /// <summary> 育成したキャラクターデータを保存する処理 </summary>
    public bool TryAddCharacterDict(uint baseCharacterID, TowerDefenseCharacterData addData)
    {
        if(_towerDefenseCharacterDataDict.ContainsKey(baseCharacterID))
        {
            TowerDefenseCharacterData[] towerDefenseCharacterData = _towerDefenseCharacterDataDict[baseCharacterID];

            for(int i = 0; i < _maxCharacterNum; i++)
            {
                if (towerDefenseCharacterData[i] == null)
                {
                    towerDefenseCharacterData[i] = addData;
                    return true;
                }
            }

            return false;
        }
        else
        {
            TowerDefenseCharacterData[] towerDefenseCharacterData = new TowerDefenseCharacterData[_maxCharacterNum];
            _towerDefenseCharacterDataDict.Add(baseCharacterID, towerDefenseCharacterData);
            towerDefenseCharacterData[0] = addData;
            return true;
        }
    }

    /// <summary> 育成したキャラクターデータを取得する処理 </summary>
    public bool TryGetCharacterDict(out TowerDefenseCharacterData getData, uint baseCharacterID, uint arrIndex)
    {
        getData = null;
        if (_towerDefenseCharacterDataDict.ContainsKey(baseCharacterID))
        {
            TowerDefenseCharacterData[] towerDefenseCharacterData = _towerDefenseCharacterDataDict[baseCharacterID];

            if (towerDefenseCharacterData[arrIndex] == null)
            {
                return false;
            }
            else
            {
                getData = towerDefenseCharacterData[arrIndex];
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}