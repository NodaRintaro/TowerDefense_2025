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

        [SerializeField] private NovelPageData[] _novelData;

        public uint EventID => _eventID;

        public NovelPageData[] NovelData => _novelData;

        public void SetID(uint id)
        {
            _eventID = id;
        }

        public void SetNovelData(NovelPageData[] novelData)
        {
            _novelData = novelData;
        }

        public bool TryGetPageData(out NovelPageData pageData, uint pageNum)
        {
            if(_novelData.Length <= pageNum)
            {
                pageData = default;
                return false;
            }

            pageData = _novelData[pageNum];
            return true;
        }
    }

    [Serializable]
    public struct NovelPageData
    {
        public string TalkCharacterName;

        public string ScenarioData;

        public string CharacterCenter;

        public string CharacterLeftBottom;

        public string CharacterRightBottom;

        public string CharacterLeftTop;

        public string CharacterRightTop;
    }
}
