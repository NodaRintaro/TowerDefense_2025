using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Rooton.Maple.Editor
{
    /// <summary>
    /// エンジンタグを自動的に割り当て、タグ名を定義してあるクラスを生成する
    /// </summary>
    public class EngineTagDetector
    {
        [MenuItem("Rooton/Maple/DetectEngineTags")]
        private static void Init()
        {
            var content = new List<string>();

            var path = "Assets/RaisingSimulation/AddressableAssetsSaveFolder/AAGDefaultLocalGroup.cs";
            var fs = File.Open(path, 
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite);

            content.Add("// 自動生成のソースコードです");
            content.Add($"public class UtilEngineTag \n");
            content.Add("{\n");

            foreach (var tag in UnityEditorInternal.InternalEditorUtility.tags)
            {
                content.Add($"public const string k{tag.Replace(" ", "")} = \"{tag}\";\n");
            }

            content.Add("}\n");

            // ファイルに書き込む
            File.WriteAllLines(path, content);
            fs.Flush();
            fs.Close();
        }
    }
}