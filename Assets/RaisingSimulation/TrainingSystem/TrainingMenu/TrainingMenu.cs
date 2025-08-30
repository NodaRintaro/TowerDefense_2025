using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrainingMenu : ITraining
{
    [SerializeField,Header("�g���[�j���O�̎��")] 
    private TrainingType _trainingType;

    [SerializeField, Header("�g���[�j���O�ɂ���ď㏸����X�e�[�^�X�ꗗ")] 
    private List<CharacterStatusBuff> _trainingBuffList;

    private uint _bonusEnhanceNum;

    public TrainingType TrainingType => _trainingType;

    public void SetBonusEnhance(uint bonusNum) => _bonusEnhanceNum = bonusNum;

    public void TrainingEvent(TrainingCharacterData trainingCharacter)
    {
        foreach (var training in _trainingBuffList)
        {
            training.BuffStatus(trainingCharacter, _bonusEnhanceNum);
        }
    }
}

