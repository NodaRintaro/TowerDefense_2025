using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SpriteData;

namespace CharacterSelectView
{
    /// <summary> CharacterSelect画面のView </summary>
    [Serializable]
    public class ViewData : IViewData
    {
        [SerializeField, Header("キャンバスのオブジェクト")] private GameObject _canvasObj;

        [SerializeField, Header("次のサポートカードセレクトの画面へ進むボタン")] private Button _nextScreenButton;

        [SerializeField, Header("ホーム画面へ進むボタン")] private Button _homeButton;

        [SerializeField, Header("キャラクターの情報を表示するUI")] private CharacterInformationView _characterInfoView;

        [SerializeField, Header("キャラクター選択を行うUIのまとめクラス")] private CharacterSelectButtonHolder _characterSelectButtonHolder;

        /// <summary> キャンバスのオブジェクト </summary> </summary>
        public GameObject CanvasObj => _canvasObj;

        /// <summary> 次のサポートカードセレクトの画面へ進むボタン </summary>
        public Button NextScreenButton => _nextScreenButton;

        /// <summary> ホーム画面へ進むボタン </summary>
        public Button HomeButton => _homeButton;

        /// <summary> キャラクターの情報を表示するUI </summary>
        public CharacterInformationView CharacterInformation => _characterInfoView;

        /// <summary> キャラクター選択を行うUIのまとめクラス </summary>
        public CharacterSelectButtonHolder CharacterSelectButtonHolder => _characterSelectButtonHolder;
    }

    #region CharacterInformation
    /// <summary> キャラクターのデータを表示するUIをまとめるクラス </summary>
    [Serializable]
    public class CharacterInformationView
    {
        [SerializeField] private Image _characterImage;

        [SerializeField] private TMP_Text _nameText = null;

        [SerializeField] private TMP_Text _idText = null;

        [SerializeField] private TMP_Text _roleTypeText = null;

        [SerializeField] private GridLayoutGroup _paramGrid = null;

        [SerializeField] private CharacterParameterUI[] _parameterUIArray= null;

        #region プロパティ一覧
        public Image CharacterImage => _characterImage;
        public TMP_Text NameText => _nameText;
        public TMP_Text IdText => _idText;
        public TMP_Text RoleTypeText => _roleTypeText;
        public GridLayoutGroup ParamGrid => _paramGrid;
        public CharacterParameterUI[] ParameterUIArray => _parameterUIArray;
        #endregion

        /// <summary> パラメータのViewへの反映を行う関数 </summary>
        public void SetParameter(ParameterType type, RankSprite paramRankSprite, uint paramNum, uint maxParamNum)
        {
            CharacterParameterUI paramUI = GetParamUI(type);
            paramUI.ParamRankImage.sprite = paramRankSprite.Sprite;
            paramUI.ParamText.text = paramNum.ToString() + "\n /" + maxParamNum.ToString();
        }

        /// <summary> パラメータUIの取得関数 </summary>
        public CharacterParameterUI GetParamUI(ParameterType type)
        {
            foreach(var param in ParameterUIArray)
            {
                if(param.ParamType == type)
                {
                    return param;
                }
            }

            return default(CharacterParameterUI);
        }

        /// <summary> パラメータUIのまとめクラス </summary>
        [Serializable]
        public struct CharacterParameterUI
        {
            [Header("ParameterのGameObject")]
            public GameObject ParamObject;

            [Header("ParameterType")]
            public ParameterType ParamType;

            public Slider ParamGage;
            public TMP_Text ParamText;
            public Image ParamRankImage;
        }
    }
    #endregion

    #region CharacterSelectMenu
    /// <summary> キャラクター選択ボタンをまとめるクラス </summary>
    [Serializable]
    public class CharacterSelectButtonHolder
    {
        [SerializeField, Header("キャラクターの選択ボタンをまとめるGridLayoutGroup")]
        private GridLayoutGroup _characterSelectButtonGLG = null;

        private Transform _gridLayoutGroupObjectTransform => _characterSelectButtonGLG.transform;

        public GridLayoutGroup CharacterSelectButtonGLG => _characterSelectButtonGLG;

        public void AddSelectButton(Button button)
        {
            button.gameObject.transform.SetParent(_gridLayoutGroupObjectTransform, true);
        }
    }
    #endregion
}