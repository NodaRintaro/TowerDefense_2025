using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingEventView : IUiView
{
    [SerializeField, Header("Canvasのオブジェクト")]
    GameObject _viewCanvas;

    public GameObject ViewCanvasObj => _viewCanvas;
}
