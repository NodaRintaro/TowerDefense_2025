using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerDefenseCharacterDataBase : IJsonSaveData
{
    /// <summary> key = BaseCharacterID, Value = 同じベースキャラクターの配列 </summary>
    public readonly Dictionary<uint, TowerDefenseCharacterData[]> TowerDefenseCharacterDataDict = new Dictionary<uint, TowerDefenseCharacterData[]>();

    private const uint _maxCharacterNum = 20;

    /// <summary> 育成したキャラクターデータを保存する処理 </summary>
    public bool TryAddCharacterDict(uint baseCharacterID, TowerDefenseCharacterData addData)
    {
        if(TowerDefenseCharacterDataDict.ContainsKey(baseCharacterID))
        {
            TowerDefenseCharacterData[] towerDefenseCharacterData = TowerDefenseCharacterDataDict[baseCharacterID];

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
            TowerDefenseCharacterDataDict.Add(baseCharacterID, towerDefenseCharacterData);
            towerDefenseCharacterData[0] = addData;
            return true;
        }
    }

    /// <summary> 育成したキャラクターデータを取得する処理 </summary>
    public bool TryGetCharacterDict(out TowerDefenseCharacterData getData, uint baseCharacterID, uint arrIndex)
    {
        getData = null;
        if (TowerDefenseCharacterDataDict.ContainsKey(baseCharacterID))
        {
            TowerDefenseCharacterData[] towerDefenseCharacterData = TowerDefenseCharacterDataDict[baseCharacterID];

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