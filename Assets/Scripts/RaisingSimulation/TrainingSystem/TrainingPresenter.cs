using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingPresenter : MonoBehaviour
{
    private TrainingManager _trainingManager;
    private TrainingView _trainingView;

    public void Start()
    {
        _trainingManager = FindAnyObjectByType<TrainingManager>();
        _trainingView = FindAnyObjectByType<TrainingView>();
    }
}
