using System;
using System.IO;
using UnityEngine;

namespace TD.Map
{
    /// <summary>
    /// MapDefinition を JSONで保存/読み込みする。
    /// 読み込み優先：persistentDataPath（自作）→ StreamingAssets（同梱）
    /// </summary>
    public static class MapSerializer
    {
        private const string MapsFolderName = "Maps";
        private const string IndexFileName = "index.json";

        private static string UserMapsDir => Path.Combine(Application.persistentDataPath, MapsFolderName);
        private static string UserIndexPath => Path.Combine(UserMapsDir, IndexFileName);

        /// <summary>
        /// IDでロード（index.json からファイル名を引く）
        /// </summary>
        public static bool TryLoadUserById(int mapId, out MapDefinition def)
        {
            def = null;

            var idx = LoadUserIndex();
            string fileName = null;
            for (int i = 0; i < idx.entries.Count; i++)
            {
                if (idx.entries[i].mapId == mapId)
                {
                    fileName = idx.entries[i].fileName;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(fileName)) return false;

            var path = Path.Combine(UserMapsDir, fileName);
            if (!File.Exists(path)) return false;

            try
            {
                var json = File.ReadAllText(path);
                def = JsonUtility.FromJson<MapDefinition>(json);
                return def != null;
            }
            catch (Exception e)
            {
                Debug.LogError($"Map load failed: {path}\n{e}");
                return false;
            }
        }

        /// <summary>
        /// index一覧を取得（UIで「ステージ名の一覧表示」に使う）
        /// </summary>
        public static MapIndex LoadUserIndexPublic() => LoadUserIndex();


        /// <summary>
        /// ステージ名で保存（ファイル名は stageName__mapId.json）。
        /// </summary>
        public static void 
            SaveUserByStageName(MapDefinition def)
        {
            if (def == null) throw new ArgumentNullException(nameof(def));
            if (string.IsNullOrWhiteSpace(def.mapId.ToString()))
                def.mapId = 0; // IDが無ければ生成

            if (string.IsNullOrWhiteSpace(def.mapName))
                def.mapName = "Untitled";

            var safe = ToSafeFileName(def.mapName);
            var fileName = $"{safe}__{def.mapId}.json";
            var filePath = Path.Combine(UserMapsDir, fileName);

            // map本体保存
            var mapJson = JsonUtility.ToJson(def, true);
            File.WriteAllText(filePath, mapJson);

            // index更新
            var idx = LoadUserIndex();
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var found = false;
            for (int i = 0; i < idx.entries.Count; i++)
            {
                if (idx.entries[i].mapId == def.mapId)
                {
                    idx.entries[i].stageName = def.mapName;
                    idx.entries[i].fileName = fileName;
                    idx.entries[i].updatedAtUnix = now;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                idx.entries.Add(new MapIndexEntry
                {
                    mapId = def.mapId, stageName = def.mapName, fileName = fileName, updatedAtUnix = now
                });
            }

            SaveUserIndex(idx);
        }

        private static MapDefinition LoadFromFile(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
                return JsonUtility.FromJson<MapDefinition>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Map load failed: {path}\n{e}");
                return null;
            }
        }

        private static void SaveUserIndex(MapIndex idx)
        {
            if (!Directory.Exists(UserMapsDir))
                Directory.CreateDirectory(UserMapsDir);

            var json = JsonUtility.ToJson(idx, true);
            File.WriteAllText(UserIndexPath, json);
        }

        private static MapIndex LoadUserIndex()
        {
            try
            {
                if (!Directory.Exists(UserMapsDir))
                    Directory.CreateDirectory(UserMapsDir);

                if (!File.Exists(UserIndexPath))
                    return new MapIndex();

                var json = File.ReadAllText(UserIndexPath);
                var idx = JsonUtility.FromJson<MapIndex>(json);
                return idx ?? new MapIndex();
            }
            catch (Exception e)
            {
                Debug.LogError($"Index load failed: {UserIndexPath}\n{e}");
                return new MapIndex();
            }
        }


        /// <summary>ステージ名をファイル名に安全にする（簡易slug）。</summary>
        private static string ToSafeFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "Untitled";

            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');

            name = name.Trim();
            if (name.Length > 40) name = name.Substring(0, 40); // 長すぎ防止（任意）
            return name;
        }
    }
}