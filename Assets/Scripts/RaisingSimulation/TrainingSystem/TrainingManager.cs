using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    #region TrainingCharacterData
    [System.Serializable]
    public class TrainingCharacterData
    {
        CharacterData _baseCharacterData;

        [SerializeField]
        private uint _currentPhysical;

        [SerializeField]
        private uint _currentPower;

        [SerializeField]
        private uint _currentIntelligence;

        [SerializeField]
        private uint _currentSpeed;

        public uint CurrentPhysical => _currentPhysical;

        public uint CurrentPower => _currentPower;

        public uint CurrentIntelligence => _currentIntelligence;

        public uint CurrentSpeed => _currentSpeed;

        public CharacterData BaseCharacterData => _baseCharacterData;

        public void SetBaseCharacter(CharacterData baseCharacter)
        {
            _baseCharacterData = baseCharacter;
        }

        public void AddCurrentPhysical(uint physical)
        {
            _currentPhysical += physical;
        }

        public void AddCurrentPower(uint power) 
        {
            _currentPower += power;
        }

        public void AddCurrentIntelligence(uint intelligence)
        {
            _currentIntelligence += intelligence;
        }

        public void AddCurrentSpeed(uint speed)
        {
            _currentSpeed += speed;
        }
    }
    #endregion

    [SerializeField,Header("キャラクターのベースデータの保存クラス")]
    private CharacterDataList _characterDataList = default;

    /// <summary> トレーニングするキャラクターのベースデータ </summary>
    private TrainingCharacterData _currentTrainigCharacter = default;

    public void StartTrainig(int characterID)
    {
        _currentTrainigCharacter = new();

        _currentTrainigCharacter.SetBaseCharacter(CharacterDataFind(characterID));
    }

    public void FinishTraining()
    {

    }

    /// <summary> IDからCharacterDataを探す処理 </summary>
    private CharacterData CharacterDataFind(int characterID)
    {
        CharacterData characterData = null;
        foreach (var character in _characterDataList.DataList)
        {
            if (character.ID == characterID)
            {
                characterData = character;
            }
        }
        return characterData;
    }
}
