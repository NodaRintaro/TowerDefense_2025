using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CharacterData;

namespace DataGenerateEditor
{
    public class SpriteDataGenerateWindow : EditorWindow
    {
        private Object _newSpriteData = null;

        private SpriteDataType _dataType;

        private int _spriteID = 0;

        private string _scriptableObjectFilePath = null;

        private const uint _spaceSize = 30;

        private bool _isSelectGenerateDataScreen = true;

        [MenuItem("Tools/GenerateScriptableObject")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(GenerateScriptableObjectMenu));
        }

        private void OnGUI()
        {
            if (_isSelectGenerateDataScreen)
            {
                //生成するDataを選択
                EditorGUILayout.Space(_spaceSize);
                EditorGUILayout.LabelField("生成するDataのタイプを選択");
                _dataType = (SpriteDataType)EditorGUILayout.EnumPopup("GenerateDataType", _dataType);

                EditorGUILayout.Space(_spaceSize);
                if (GUILayout.Button("CreateDataInstance"))
                {
                    _isSelectGenerateDataScreen = false;
                }
            }
            else
            {
                DrawGenerateDataWindow(_dataType);
            }
        }

        private void DrawGenerateDataWindow(SpriteDataType spriteDataType)
        {
            switch (spriteDataType)
            {
                case SpriteDataType.Character:
                    _scriptableObjectFilePath = "Assets/Resources_moved/CharacterData/SpriteData";
                    DrawNewCharacterSpriteData();
                    break;
                case SpriteDataType.SupportCard:
                    DrawNewSupportCardSpriteData();
                    break;
            }
        }

        private void GenerateSpriteData(Object spData)
        {
            AssetDataCreate(spData);
        }

        private void AssetDataCreate(Object data)
        {
            // アセットとして保存
            AssetDatabase.CreateAsset(data, _scriptableObjectFilePath);
            AssetDatabase.SaveAssets();

            //AssetDataBaseの内容を更新
            AssetDatabase.Refresh();
        }

        private void DrawNewCharacterSpriteData()
        {
            _newSpriteData = new CharacterSpriteData();

            //TextFieldで自動生成するScriptableObjectの名前を入力
            EditorGUILayout.Space(_spaceSize);
            EditorGUILayout.LabelField("自動生成するScriptableObjectの名前");
            _spriteID = EditorGUILayout.IntField("SpriteID", _spriteID);
        }

        private void DrawNewSupportCardSpriteData()
        {

        }
    }
}