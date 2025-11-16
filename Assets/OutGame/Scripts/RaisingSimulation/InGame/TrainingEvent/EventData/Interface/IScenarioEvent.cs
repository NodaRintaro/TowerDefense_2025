using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScenarioEvent
{
    public void ScenarioLoad();

    public void StartNovelEvent();

    public void FinishEvent();
}
