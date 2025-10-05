using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingEventUIHolder : ITrainigUiHolder
{
    [SerializeField, Header("Canvas�̃I�u�W�F�N�g")]
    GameObject _viewCanvas;

    [SerializeField, Header("�L�����N�^�[�̃X�v���C�g")]
    Sprite _characterSprite = null;

    [SerializeField, Header("���b�Z�[�W�̃e�L�X�g")]
    private TMP_Text _textUi = default;

    public TMP_Text TextUI => _textUi;
    public GameObject ViewCanvasObj => _viewCanvas;
}
