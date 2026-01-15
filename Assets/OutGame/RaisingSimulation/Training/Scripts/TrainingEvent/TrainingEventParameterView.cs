using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Add this line

public class TrainingEventParameterView : MonoBehaviour
{
    [SerializeField]private GameObject _parameterObj;
    private Transform _parameterOriginalTransform;

    [Header("　キャラクターの各パラメータUI ")]

    [SerializeField, Header("筋力")]
    private CharacterParameterUI _powerParameterUI = null;
    [SerializeField, Header("知力")]
    private CharacterParameterUI _intelligenceParameterUI = null;
    [SerializeField, Header("体力")]
    private CharacterParameterUI _physicalParameterUI = null;
    [SerializeField, Header("素早さ")]
    private CharacterParameterUI _speedParameterUI = null;

    public GameObject ParameterObj => _parameterObj;
    public CharacterParameterUI PowerParameterUI => _powerParameterUI;
    public CharacterParameterUI IntelligenceParameterUI => _intelligenceParameterUI;
    public CharacterParameterUI PhysicalParameterUI => _physicalParameterUI;
    public CharacterParameterUI SpeedParameterUI => _speedParameterUI;

    public void Start()
    {
        _parameterOriginalTransform = transform;
    }

    /// <summary> 元居た場所に戻る処理 </summary>
    public void SetOriginalTransform()
    {
        _parameterObj.transform.position = _parameterObj.transform.position;
    }
}
