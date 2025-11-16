using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventNovelData
{
    [SerializeField, Header("イベントのID")]
    private uint _eventID;

    [SerializeField, Header("背景のID")]
    private uint _backScreenID;

    [SerializeField, Header("シナリオのなかみ")]
    private ScenarioData[] _scenarioArr;

    public uint EventID => _eventID;
    public ScenarioData[] ScenarioArr => _scenarioArr;

    public void InitData(uint eventID , ScenarioData[] scenarioData)
    {
        _eventID = eventID;
        _scenarioArr = scenarioData;
    }

    [Serializable]
    public class ScenarioData
    {
        private string _scenario;

        private uint _speakCharacterID;

        private uint _rightCharacterID;

        private uint _leftCharacterID;

        public string Scenario => _scenario;
        public uint SpeakCharacterID => _speakCharacterID;
        public uint RightCharacterID => _rightCharacterID;
        public uint LeftCharacterID => _leftCharacterID;

        public void SetData(uint speakCharacter, string scenario)
        {
            _speakCharacterID = speakCharacter;
            _scenario = scenario;
        }

        public void SetRightCharacter(uint rightCharacter)
        {
            _rightCharacterID = rightCharacter;
        }

        public void SetLefCharacter(uint lefCharacter)
        {
            _leftCharacterID = lefCharacter;
        }
    }
}




