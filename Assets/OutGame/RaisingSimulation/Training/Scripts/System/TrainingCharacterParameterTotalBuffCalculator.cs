using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// トレーニングキャラクターの強化を行うクラス
/// </summary>
public class TrainingCharacterParameterTotalBuffCalculator
{
    private TrainingCharacterData _trainingCharacterData;

    //ベースの数値
    private uint _basePowerBuff = 0;
    private uint _baseIntelligenceBuff = 0;
    private uint _basePhysicalBuff = 0;
    private uint _baseSpeedBuff = 0;

    //ベース数値に対してのパーセンテージバフ
    private uint _powerPercentageBuff = 0;
    private uint _intelligencePercentageBuff = 0;
    private uint _physicalPercentageBuff = 0;
    private uint _speedPercentageBuff = 0;

    //シンプルな足し算バフ
    private int _powerPlusBuff = 0;
    private int _intelligencePlusBuff = 0;
    private int _physicalPlusBuff = 0;
    private int _speedPlusBuff = 0;

    [Inject]
    public TrainingCharacterParameterTotalBuffCalculator(JsonTrainingSaveDataRepository dataRepository)
    {
        _trainingCharacterData = dataRepository.RepositoryData.TrainingCharacterData;
    }

    /// <summary> トレーニングキャラクターの強化を行う処理 </summary>
    public void TotalBuffTrainingCharacter()
    {
        _trainingCharacterData.AddCurrentPower(TotalBuffCalculate(_basePowerBuff, _powerPercentageBuff, _powerPlusBuff));
        _trainingCharacterData.AddCurrentIntelligence(TotalBuffCalculate(_baseIntelligenceBuff, _intelligencePercentageBuff, _intelligencePlusBuff));
        _trainingCharacterData.AddCurrentPhysical(TotalBuffCalculate(_basePhysicalBuff, _physicalPercentageBuff, _physicalPlusBuff));
        _trainingCharacterData.AddCurrentSpeed(TotalBuffCalculate(_baseSpeedBuff, _speedPercentageBuff, _speedPlusBuff));

        InitBuff();
    }

    /// <summary> バフのベースとなる数値を決める処理 </summary>
    public void SetBaseBuff(uint power, uint intelligence, uint physical, uint speed)
    {
        _basePowerBuff = power;
        _baseIntelligenceBuff = intelligence;
        _basePhysicalBuff = physical;
        _baseSpeedBuff = speed;
    }

    /// <summary> ベース数値に対してのパーセンテージバフの値を増加する処理 </summary>
    public void AddPercentageBuff(uint power, uint intelligence, uint physical, uint speed)
    {
        _powerPercentageBuff += power;
        _intelligencePercentageBuff += intelligence;
        _physicalPercentageBuff += physical;
        _speedPercentageBuff += speed;
    }

    /// <summary> 足し算バフの値を増加する処理 </summary>
    public void AddPlusBuff(int power, int intelligence, int physical, int speed)
    {
        _powerPlusBuff += power;
        _intelligencePlusBuff += intelligence;
        _physicalPlusBuff += physical;
        _speedPlusBuff += speed;
    }

    public void TrainingLevelBonus(uint powerTrainingLevel, uint intelligenceTrainingLevel, uint physicalTrainingLevel, uint speedTrainingLevel)
    {
        _basePowerBuff *= powerTrainingLevel;
        _baseIntelligenceBuff *= intelligenceTrainingLevel;
        _basePhysicalBuff *= physicalTrainingLevel;
        _baseSpeedBuff *= speedTrainingLevel;
    }

    /// <summary> Buff全体の合計値 </summary>
    private uint TotalBuffCalculate(uint baseNum, uint percentageNum, int plusNum)
    {
        uint percentageTotalBuff = baseNum * (percentageNum % 100);
        uint totalNum = baseNum + percentageTotalBuff + (uint)plusNum;
        return totalNum;
    }

    /// <summary> Buff全体の初期化 </summary>
    private void InitBuff()
    {
        //ベースの数値
        _basePowerBuff = 0;
        _baseIntelligenceBuff = 0;
        _basePhysicalBuff = 0;
        _baseSpeedBuff = 0;

        //ベース数値に対してのパーセンテージバフ
        _powerPercentageBuff = 0;
        _intelligencePercentageBuff = 0;
        _physicalPercentageBuff = 0;
        _speedPercentageBuff = 0;

        //シンプルな足し算バフ
        _powerPlusBuff = 0;
        _intelligencePlusBuff = 0;
        _physicalPlusBuff = 0;
        _speedPlusBuff = 0;
    }
}
