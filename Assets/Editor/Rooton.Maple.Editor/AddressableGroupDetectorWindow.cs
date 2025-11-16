using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Rooton.Maple.Editor
{
    /// <summary>
    /// AddressableGroupを自動的に割り当て、グループ名を定義してあるクラスを生成する
    /// </summary>
    public class AddressableGroupDetectorWindow : EditorWindow
    {
        private AddressableAssetGroup _targetGroup;
        private string _saveFolder;

        [MenuItem("Rooton/Maple/AddressableGroupDetectorWindow")]
        private static void Init()
        {
            CreateWindow<AddressableGroupDetectorWindow>().Show();
        }

        private void OnGUI()
        {
            _targetGroup =
                (AddressableAssetGroup)EditorGUILayout.ObjectField(_targetGroup, typeof(AddressableAssetGroup), false);
            _saveFolder = EditorGUILayout.TextField("SavePath", _saveFolder);
            if (_targetGroup == null) return;
            var path = _saveFolder + $"/AAG{_targetGroup.name.Replace(" ", "")}.cs";

            if (GUILayout.Button("Generate"))
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                }

                List<string> content = new List<string>();
                content.Add("// 自動生成のソースコードです");
                content.Add($"public class AAG{_targetGroup.name.Replace(" ", "")}" + "\n{\n");
                foreach (var obj in _targetGroup.entries)
                {
                    var line =
                        $"public const string k{obj.AssetPath.Split('.')[0].Replace("/", "_")} = \"{obj.AssetPath}\";\n";
                    content.Add(line);
                }

                content.Add("}\n");
                File.WriteAllLines(path, content, System.Text.Encoding.UTF8);
            }
        }
    }
}