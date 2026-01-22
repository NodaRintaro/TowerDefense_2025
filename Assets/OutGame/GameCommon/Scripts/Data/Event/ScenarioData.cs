using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ScenarioData
{
    [SerializeField] private Queue<NovelPageData> _novelData =new();

    public Queue<NovelPageData> NovelData => _novelData;

    public void EnQueuePageData(NovelPageData novelPageData)
    {
        _novelData.Enqueue(novelPageData);
    }

    public bool TryGetNextPage(out NovelPageData novelPageData)
    {
        novelPageData = new();

        //ページが残っていなければFalseを返す
        if(_novelData.Count == 0) return false;
        
        //残っていればページを渡す
        novelPageData = _novelData.Dequeue();
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

    public string BackScreenName;
}
