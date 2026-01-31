using Newtonsoft.Json;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary> キャラクターのベースデータ </summary>
#region CharacterBaseData
[Serializable]
public class CharacterBaseData
{
    //ステータス
    [SerializeField, Header("ID"), JsonProperty]
    protected uint _characterID;
    [SerializeField, Header("名前"), JsonProperty]
    protected string _characterName;
    [SerializeField, Header("レア度"), JsonProperty]
    protected uint _baseRarity;
    [SerializeField, Header("体力"), JsonProperty]
    protected uint _basePhysical;
    [SerializeField, Header("筋力"), JsonProperty]
    protected uint _basePower;
    [SerializeField, Header("知力"), JsonProperty]
    protected uint _baseIntelligence;
    [SerializeField, Header("素早さ"), JsonProperty]
    protected uint _baseSpeed;
    [SerializeField, Header("戦闘スタイル"), JsonProperty]
    protected JobType _roleType;
    [SerializeField, Header("コスト"), JsonProperty]
    protected uint _cost;
    [SerializeField, Header("スキルのID"), JsonProperty]
    protected uint _skillID;
    [SerializeField, Header("キャラクターのイメージデータ"), JsonProperty]
    protected CharacterImageData _imageData;
    public uint CharacterID => _characterID;
    public string CharacterName => _characterName;
    public uint BaseRarity => _baseRarity;
    public uint BasePhysical => _basePhysical;
    public uint BasePower => _basePower;
    public uint BaseIntelligence => _baseIntelligence;
    public uint BaseSpeed => _baseSpeed;
    public JobType CharacterRole => _roleType;
    public uint Cost => _cost;
    public uint SkillID => _skillID;
    public CharacterImageData CharacterImageData => _imageData;

    #region パラメータの合計値の参照プロパティ
    public virtual uint TotalPhysical => _basePhysical;
    public virtual uint TotalPower => _basePower;
    public virtual uint TotalIntelligence => _baseIntelligence;
    public virtual uint TotalSpeed => _baseSpeed;
    #endregion

    /// <summary>
    /// パラメータの初期化関数
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="charName">名前</param>
    /// <param name="rarity">レアリティ</param>
    /// <param name="phys">体力</param>
    /// <param name="pow">筋力</param>
    /// <param name="intelligence">知力</param>
    /// <param name="sp">素早さ</param>
    public void InitData(uint id, string charName, uint rarity, uint phys, uint pow, uint intelligence, uint sp, string role, uint cost, uint skillID, CharacterImageData characterImageData)
    {
        _characterID = id;
        _characterName = charName;
        _baseRarity = rarity;
        _basePhysical = phys;
        _basePower = pow;
        _baseIntelligence = intelligence;
        _baseSpeed = sp;
        SetCharacterRole(role);
        _cost = cost;
        _skillID = skillID;
        _imageData = characterImageData;
    }

    /// <summary>
    /// ベースデータの登録関数
    /// </summary>
    /// <param name="baseData"></param>
    public void SetBaseData(CharacterBaseData baseData)
    {
        _characterID = baseData.CharacterID;
        _characterName = baseData.CharacterName;
        _baseRarity = baseData.BaseRarity;
        _basePhysical = baseData.BasePhysical;
        _basePower = baseData.BasePower;
        _baseIntelligence = baseData.BaseIntelligence;
        _baseSpeed = baseData.BaseSpeed;
        _roleType = baseData.CharacterRole;
        _cost = baseData.Cost;
    }

    protected void SetCharacterRole(string roleType)
    {
        foreach (string role in Enum.GetNames(typeof(JobType)))
        {
            if (role == roleType)
            {
                Enum.TryParse(role, out _roleType);
            }
        }
    }
}
#endregion



