using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// トレーニングキャラクターの強化を行うクラス
/// </summary>
public class ParameterBuffCalculator
{
    private TrainingCharacterData _trainingCharacterData;

    //ベースの数値
    private int _basePowerBuff = 0;
    private int _baseIntelligenceBuff = 0;
    private int _basePhysicalBuff = 0;
    private int _baseSpeedBuff = 0;

    //ベース数値に対してのパーセンテージバフ
    private int _powerPercentageBuff = 0;
    private int _intelligencePercentageBuff = 0;
    private int _physicalPercentageBuff = 0;
    private int _speedPercentageBuff = 0;

    //シンプルな足し算バフ
    private int _powerPlusBuff = 0;
    private int _intelligencePlusBuff = 0;
    private int _physicalPlusBuff = 0;
    private int _speedPlusBuff = 0;

    [Inject]
    public ParameterBuffCalculator(JsonTrainingSaveDataRepository dataRepository)
    {
        _trainingCharacterData = dataRepository.RepositoryData.TrainingCharacterData;
    }

    public bool IsTotalBuffZero()
    {
        int totalBuff = TotalBuffCalculate(_basePowerBuff, _powerPercentageBuff, _powerPlusBuff);
        totalBuff += TotalBuffCalculate(_baseIntelligenceBuff, _intelligencePercentageBuff, _intelligencePlusBuff);
        totalBuff += TotalBuffCalculate(_basePhysicalBuff, _physicalPercentageBuff, _physicalPlusBuff);
        totalBuff += TotalBuffCalculate(_baseSpeedBuff, _speedPercentageBuff, _speedPlusBuff);

        if (_powerPlusBuff != 0) return false;

        return true;
    }

    /// <summary> 育成イベントのバフ </summary>
    public void OnTrainingEventBuff()
    {
        int totalPowerBuff = TotalBuffCalculate(_basePowerBuff, _powerPercentageBuff, _powerPlusBuff);
        int totalIntelligenceBuff = TotalBuffCalculate(_baseIntelligenceBuff, _intelligencePercentageBuff, _intelligencePlusBuff);
        int totalPhysicalBuff = TotalBuffCalculate(_basePhysicalBuff, _physicalPercentageBuff, _physicalPlusBuff);
        int totalSpeedBuff = TotalBuffCalculate(_baseSpeedBuff, _speedPercentageBuff, _speedPlusBuff);

        if (totalPowerBuff < 0)
            _trainingCharacterData.DecreaseCurrentPower((uint)Math.Abs(totalPowerBuff));
        else if(totalPowerBuff > 0)
            _trainingCharacterData.AddCurrentPower((uint)totalPowerBuff);

        if (totalIntelligenceBuff < 0)
            _trainingCharacterData.DecreaseCurrentPower((uint)Math.Abs(totalIntelligenceBuff));
        else if (totalIntelligenceBuff > 0)
            _trainingCharacterData.AddCurrentPower((uint)totalIntelligenceBuff);

        if (totalPhysicalBuff < 0)
            _trainingCharacterData.DecreaseCurrentPower((uint)Math.Abs(totalPhysicalBuff));
        else if (totalPhysicalBuff > 0)
            _trainingCharacterData.AddCurrentPower((uint)totalPhysicalBuff);

        if (totalSpeedBuff < 0)
            _trainingCharacterData.DecreaseCurrentPower((uint)Math.Abs(totalSpeedBuff));
        else if (totalSpeedBuff > 0)
            _trainingCharacterData.AddCurrentPower((uint)totalSpeedBuff);
    }

    /// <summary> バフのベースとなる数値を決める処理 </summary>
    public void SetTrainingEventBuff(ITrainingEventData trainingEventData)
    {
        _basePowerBuff = trainingEventData.PowerBaseBuff;
        _baseIntelligenceBuff = trainingEventData.IntelligenceBaseBuff;
        _basePhysicalBuff = trainingEventData.PhysicalBaseBuff;
        _baseSpeedBuff = trainingEventData.SpeedBaseBuff;
    }

    /// <summary> トレーニングのそれぞれのレベルに応じてベースの強化数値を上げる処理 </summary>
    public void SetTrainingLevelBonus(int powerTrainingLevel, int intelligenceTrainingLevel, int physicalTrainingLevel, int speedTrainingLevel)
    {
        _basePowerBuff *= powerTrainingLevel;
        _baseIntelligenceBuff *= intelligenceTrainingLevel;
        _basePhysicalBuff *= physicalTrainingLevel;
        _baseSpeedBuff *= speedTrainingLevel;
    }

    /// <summary> 足し算バフの値を増加する処理 </summary>
    public void PlusBuffCalculate(int power, int intelligence, int physical, int speed)
    {
        _powerPlusBuff += power;
        _intelligencePlusBuff += intelligence;
        _physicalPlusBuff += physical;
        _speedPlusBuff += speed;
    }

    /// <summary> ベース数値に対してのパーセンテージバフの値を増加する処理 </summary>
    public void PercentageBuffCalculate(int power, int intelligence, int physical, int speed)
    {
        _powerPercentageBuff += power;
        _intelligencePercentageBuff += intelligence;
        _physicalPercentageBuff += physical;
        _speedPercentageBuff += speed;
    }

    /// <summary> Buff全体の合計値 </summary>
    public int TotalBuffCalculate(int baseNum, int percentageNum, int plusNum)
    {
        int percentageTotalBuff = baseNum * (percentageNum % 100);
        int totalNum = baseNum + percentageTotalBuff + plusNum;
        return totalNum;
    }

    /// <summary> Buff全体の初期化 </summary>
    public void DeleteBuff()
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
