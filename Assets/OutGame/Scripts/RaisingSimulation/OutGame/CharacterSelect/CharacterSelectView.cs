using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterSelectView
{
    public struct ViewData
    {
        /// <summary> 次のサポートカードセレクトの画面へ進むボタン </summary>
        public readonly Button NextScreenButton;

        /// <summary> ホーム画面へ進むボタン </summary>
        public readonly Button HomeButton;

        /// <summary> キャラクターの情報を表示するUI </summary>
        public readonly CharacterInformationView CharacterInformation;

        /// <summary> キャラクター選択を行うUIのまとめクラス </summary>
        public readonly CharacterSelectMenuView CharacterSelectMenu;
    }

    #region CharacterInformation
    [Serializable]
    public class CharacterInformationView
    {
        [SerializeField] private readonly Image _characterImage;

        [SerializeField] private readonly TMP_Text _nameText = null;

        [SerializeField] private readonly TMP_Text _idText = null;

        [SerializeField] private readonly TMP_Text _roleTypeText = null;

        [SerializeField] private readonly GridLayoutGroup _paramGrid = null;

        [SerializeField] private CharacterParameterUI[] _parameterUIArray= null;

        /// <summary>
        /// キャラクターの情報を表示するUIDataをまとめるクラス
        /// </summary>
        /// <param name="characterImage">キャラクターの立ち絵</param>
        /// <param name="nameText">名前</param>
        /// <param name="idText">ID</param>
        /// <param name="roleTypeText">役職</param>
        /// <param name="_paramGrid">パラメータをまとめるGridLayoutGroup</param>
        /// <param name="rankDataHolder">各種ランクのデータ</param>
        public CharacterInformationView(Image characterImage, TMP_Text nameText, TMP_Text idText, TMP_Text roleTypeText, GridLayoutGroup paramGrid, RankSpriteDataHolder rankDataHolder, CharacterParameterUI[] parameters)
        {
            //UIの情報を保存
            _characterImage = characterImage;
            _nameText = nameText;
            _idText = idText;
            _roleTypeText = roleTypeText;
            _paramGrid = paramGrid;
            _parameterUIArray = parameters;
        }

        public void SetCharacterInformation(CharacterBaseData characterBaseData, Sprite characterSprite)
        {
            _characterImage.sprite = characterSprite;
            _nameText.text = characterBaseData.CharacterName;
            _idText.text = characterBaseData.CharacterID.ToString();
            _roleTypeText.text = characterBaseData.RoleType.ToString();

            foreach(var paramUI in _parameterUIArray)
            {
                switch (paramUI.ParamType)
                {
                    case ParameterType.Physical:
                        SetParameter(characterBaseData.BasePhysical, paramUI);
                        break;
                    case ParameterType.Power:
                        SetParameter(characterBaseData.BasePower, paramUI);
                        break;
                    case ParameterType.Intelligence:
                        SetParameter(characterBaseData.BaseIntelligence, paramUI);
                        break;
                    case ParameterType.Speed:
                        SetParameter(characterBaseData.BaseSpeed, paramUI);
                        break;
                }
            }
        }

        private void SetParameter(uint characterParam, CharacterParameterUI paramUI)
        {
            
        }

        public struct CharacterParameterUI
        {
            public ParameterType ParamType;
            public Slider ParamGage;
            public TMP_Text ParamText;
            public TMP_Text NextRankParamNum;
            public Image ParamRankImage;
        }
    }
    #endregion

    #region CharacterSelectMenu
    public class CharacterSelectMenuView
    {
        [SerializeField, Header("キャラクターの選択ボタンをまとめるGridLayoutGroup")]
        private GridLayoutGroup _characterSelectButtonGLG = null;

        private Transform _gridLayoutGroupObjectTransform;

        public GridLayoutGroup CharacterSelectButtonGLG => _characterSelectButtonGLG;

        public CharacterSelectMenuView(GridLayoutGroup selectButtonGroup)
        {
            _characterSelectButtonGLG = selectButtonGroup;
            _gridLayoutGroupObjectTransform = selectButtonGroup.gameObject.transform;
        }

        public void AddSelectButton(Button button)
        {
            button.gameObject.transform.SetParent(_gridLayoutGroupObjectTransform, true);
        }
    }
    #endregion
}