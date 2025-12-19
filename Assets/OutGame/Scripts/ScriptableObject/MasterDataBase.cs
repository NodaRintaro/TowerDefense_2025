using CharacterData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dataの管理を行うScriptableObjectClassの継承元となる基底Class
/// </summary>
public abstract class MasterDataBase<T> : ScriptableObject
{
    [SerializeField, Header("データの保存先")]
    protected T[] _dataHolder;

    public T[] DataHolder => _dataHolder;

    public void InitData(T[] data)
    {
        _dataHolder = data;
    }
}
