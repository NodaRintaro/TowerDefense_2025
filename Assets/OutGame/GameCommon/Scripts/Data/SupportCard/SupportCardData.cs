using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SupportCardData
{
    [SerializeField, Header("ID")]
    private uint _id;
    [SerializeField, Header("名前")]
    private string _cardName;
    [SerializeField, Header("レアリティ")]
    private uint _rarity;
    [SerializeField, Header("クールタイム")]
    private float _coolTime;
    [SerializeField, Header("体力の基礎強化")]
    private int _addPhysical;
    [SerializeField, Header("筋力の基礎強化")]
    private int _addPower;
    [SerializeField, Header("知力の基礎強化")]
    private int _addIntelligence;
    [SerializeField, Header("素早さの基礎強化")]
    private int _addSpeed;
    [SerializeField, Header("体力の強化率")]
    private uint _physicalPercentage;
    [SerializeField, Header("筋力の強化率")]
    private uint _powerPercentage;
    [SerializeField, Header("知力の強化率")]
    private uint _intelligencePercentage;
    [SerializeField, Header("素早さの強化率")]
    private uint _speedPercentage;

    public uint ID => _id;
    public string CardName => _cardName;
    public uint Rarity => _rarity;
    public float CoolTime => _coolTime;
    public int AddPhysical => _addPhysical;
    public int AddPower => _addPower;
    public int AddIntelligence => _addIntelligence;
    public int AddSpeed => _addSpeed;
    public uint PhysicalPercentage => _physicalPercentage;
    public uint PowerPercentage => _powerPercentage;
    public uint IntelligencePercentage => _intelligencePercentage;
    public uint SpeedPercentage => _speedPercentage;

    /// <summary>
    /// パラメータの初期化
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="charaName">名前</param>
    /// <param name="physi">体力の強化倍率</param>
    /// <param name="pow">筋力の強化倍率</param>
    /// <param name="intelli">知力の強化倍率</param>
    /// <param name="sp">素早さの強化倍率</param>
    public void InitData(string id, string charaName, string coolTime, string addPhysi, string addPow, string addIntelli, string addSp, string physiPercentage, string powPercentage, string intelliPercentage, string spPercentage, string rarity)
    {
        _id = uint.Parse(id);
        _cardName = charaName;
        _coolTime = int.Parse(coolTime);
        _addPhysical = int.Parse(addPhysi);
        _addPower = int.Parse(addPow);
        _addIntelligence = int.Parse(addIntelli);
        _addSpeed = int.Parse(addSp);
        _physicalPercentage = uint.Parse(physiPercentage);
        _powerPercentage = uint.Parse(powPercentage);
        _intelligencePercentage = uint.Parse(intelliPercentage);
        _speedPercentage = uint.Parse(spPercentage);
        _rarity = uint.Parse(rarity);
    }
}