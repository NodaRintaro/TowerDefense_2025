using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingEventView : IUiHolder
{
    [SerializeField, Header("Canvas�̃I�u�W�F�N�g")]
    GameObject _viewCanvas;

    public GameObject ViewCanvasObj => _viewCanvas;
}
