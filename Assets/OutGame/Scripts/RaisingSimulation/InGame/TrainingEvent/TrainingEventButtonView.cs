using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TrainingEventButtonView
{
    [SerializeField, Header("ボタンのデータ")]
    private TrainingButtonData[] _trainingButtonDataArr;

    public TrainingButtonData FindButtonData(TrainingType type)
    {
        foreach (var item in _trainingButtonDataArr)
        {
            if(item.TrainingType == type)
            {
                return item;
            }
        }

        return null;
    }

    public class TrainingButtonData
    {
        private TrainingType _tariningType = default;

        private Button _trainingButton = null;

        public TrainingType TrainingType => _tariningType;
        public Button TrainingButton => _trainingButton;
    }
}
