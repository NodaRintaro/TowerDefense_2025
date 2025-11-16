using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Rooton.Maple.Editor
{
    /// <summary>
    /// エンジンレイヤーを自動的に割り当て、レイヤー名を定義してあるクラスを生成する
    /// </summary>
    public class EngineLayerDetector
    {
        [MenuItem("Rooton/Maple/DetectEngineLayers")]
        private static void Init()
        {
            var content = new List<string>();

            var path = "Assets/RaisingSimulation/AddressableAssetsSaveFolder/AAGDefaultLocalGroup.cs";
            var fs = File.Open(path, FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite);

            content.Add("// 自動生成のソースコードです");
            content.Add($"public class UtilEngineLayer \n");
            content.Add("{\n");

            foreach (var l in UnityEditorInternal.InternalEditorUtility.layers)
            {
                content.Add($"public const string k{l.Replace(" ", "")} = \"{l}\";\n");
            }

            content.Add("}\n");

            File.WriteAllLines(path, content);
            fs.Flush();
            fs.Close();
        }
    }
}