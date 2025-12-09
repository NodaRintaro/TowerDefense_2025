using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NovelEventData
{
    [Serializable]
    public class NovelEventDataHolder
    {
        private EventData[] _eventHolder;

        public EventData[] EventDataHolder => _eventHolder;
    }

    [Serializable]
    public class EventData
    {
        [SerializeField] private uint _eventID;

        [SerializeField] private NovelScreenData[] _novelData;

        public uint EventID => _eventID;

        public NovelScreenData[] NovelData => _novelData;

        public void SetData(uint id, NovelScreenData[] novelData)
        {
            _eventID = id;
            _novelData = novelData;
        }
    }

    [Serializable]
    public struct NovelScreenData
    {
        public string TalkCharacterName;

        public string ScenarioData;

        public int BackScreenID;

        public string[] StandingCharacterNames;

        public bool IsFinishTalk;
    }
}
