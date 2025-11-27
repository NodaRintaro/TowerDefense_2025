using System.Collections.Generic;
using UnityEngine;

namespace CharacterAssets
{
    [CreateAssetMenu(fileName = "CharacterDataList", menuName = "ScriptableObject/CharacterDataList")]
    public class CharacterDataHolder : ScriptableObject
    {
        [SerializeField, Header("キャラクターのデータリスト")]
        private List<CharacterBaseData> _dataList = new();

        public List<CharacterBaseData> CharacterInformationDataList => _dataList;

        public void AddData(CharacterBaseData characterData)
        {
            _dataList.Add(characterData);
        }

        public CharacterBaseData GetData(uint id)
        {
            foreach (var item in _dataList)
            {
                if (item.CharacterID == id)
                {
                    return item;
                }
            }
            return null;
        }
    }

    [CreateAssetMenu(fileName = "CharacterResource", menuName = "ScriptableObject/CharacterResource")]
    public class CharacterResourceHolder
    {
        [SerializeField, Header("キャラクターのデータリスト")]
        private List<CharacterResource> _dataList = new();

        public List<CharacterResource> CharacterInformationDataList => _dataList;

        public void AddData(CharacterResource characterData)
        {
            _dataList.Add(characterData);
        }

        public CharacterResource GetData(uint id)
        {
            foreach (var item in _dataList)
            {
                if (item.CharacterID == id)
                {
                    return item;
                }
            }
            return null;
        }
    }
}