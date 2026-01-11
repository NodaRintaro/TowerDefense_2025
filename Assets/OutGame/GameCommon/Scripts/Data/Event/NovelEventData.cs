using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NovelData
{
    [Serializable]
    public class NovelEventData
    {
        [SerializeField] private uint _eventID;

        [SerializeField] private NovelData[] _novelData;

        public uint EventID => _eventID;

        public NovelData[] NovelData => _novelData;

        public void SetID(uint id)
        {
            _eventID = id;
        }

        public void SetNovelData(NovelData[] novelData)
        {
            _novelData = novelData;
        }
    }

    [Serializable]
    public struct NovelData
    {
        public string TalkCharacterName;

        public string ScenarioData;
    }
}
