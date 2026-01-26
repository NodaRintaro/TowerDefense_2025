using System.Collections.Generic;
using UnityEngine;
using TD.Map;

namespace TD.View3D
{
    /// <summary>
    /// ルートを線（LineRenderer）と番号（TextMesh）で可視化する。
    /// </summary>
    public sealed class RouteVisualizer3D : MonoBehaviour
    {
        [Header("Refs")] [SerializeField] private TD.EditorRuntime.MapEditorController3D _controller;

        [Header("Line Settings")] [SerializeField]
        private Material _lineMaterial;

        [SerializeField] private float _lineWidth = 0.08f;
        [SerializeField] private float _activeLineWidth = 0.14f;
        [SerializeField] private float _lineY = 0.05f;

        [Header("Label Settings")] [SerializeField]
        private Font _labelFont;

        [SerializeField] private float _labelCharSize = 0.2f;
        [SerializeField] private float _labelY = 0.15f;

        [Header("3D Grid")] [SerializeField] private Vector3 _origin = Vector3.zero;
        [SerializeField] private float _tileSize = 1f;

        [Header("Roots")] [SerializeField] private Transform _lineRoot;
        [SerializeField] private Transform _labelRoot;

        private GridToWorld _g2w;

        private readonly Dictionary<string, LineRenderer> _lines = new();
        private readonly Dictionary<string, List<TextMesh>> _labels = new();

        private void Awake()
        {
            if (_lineRoot == null) _lineRoot = new GameObject("RouteLines").transform;
            if (_labelRoot == null) _labelRoot = new GameObject("RouteLabels").transform;

            _lineRoot.SetParent(transform, false);
            _labelRoot.SetParent(transform, false);

            _g2w = new GridToWorld(_origin, _tileSize);
        }

        private void Start()
        {
            RebuildAll();
        }

        public void RebuildAll()
        {
            ClearAll();
            RefreshAll();
        }

        /// <summary>ルート表示を更新（編集後に呼ぶ）</summary>
        public void RefreshAll()
        {
            if (_controller == null) return;
            var routes = _controller.Routes;
            if (routes == null) return;

            for (int i = 0; i < routes.Count; i++)
            {
                var r = routes[i];
                if (r == null || string.IsNullOrWhiteSpace(r.routeId)) continue;

                var isActive = (i == GetActiveRouteIndexFallback());
                DrawRoute(r, isActive);
            }

            // 存在しなくなったrouteIdの掃除（簡易）
            CleanupUnused(routes);
        }

        // MapEditorController3Dが_activeRouteIndexをpublicにしてないので暫定
        // もし後でプロパティ追加できるなら、それを使ってOK
        private int GetActiveRouteIndexFallback() => 0;

        private void DrawRoute(RouteDefinition r, bool isActive)
        {
            // 代表線：S0→waypoints→G0（無ければ描かない）
            var points = BuildRepresentativeLinePoints(r);

            var line = GetOrCreateLine(r.routeId);
            if (points.Count >= 2)
            {
                line.gameObject.SetActive(true);
                line.positionCount = points.Count;
                for (int i = 0; i < points.Count; i++) line.SetPosition(i, points[i]);
                line.widthMultiplier = isActive ? _activeLineWidth : _lineWidth;
            }
            else
            {
                // spawn/goalが無い等で線を引けない
                line.gameObject.SetActive(false);
            }

            // ラベル：全Spawn(S0..)、全Goal(G0..)、Waypoints(0..)
            var labelList = GetOrCreateLabels(r.routeId);

            int spawnCount = r.spawns?.Count ?? 0;
            int goalCount  = r.goals?.Count  ?? 0;
            int wpCount    = r.waypoints?.Count ?? 0;

            int need = spawnCount + wpCount + goalCount;
            EnsureLabelCount(labelList, need, r.routeId);

            int idx = 0;

            // Spawns: S0,S1...
            if (spawnCount > 0)
            {
                for (int s = 0; s < spawnCount; s++)
                {
                    var p = _g2w.GridToWorldCenter(r.spawns[s]);
                    SetLabel(labelList[idx++], $"S{s}", new Vector3(p.x, _labelY, p.z));
                }
            }

            // Waypoints: 0,1,2...
            if (wpCount > 0)
            {
                for (int w = 0; w < wpCount; w++)
                {
                    var p = _g2w.GridToWorldCenter(r.waypoints[w]);
                    SetLabel(labelList[idx++], w.ToString(), new Vector3(p.x, _labelY, p.z));
                }
            }

            // Goals: G0,G1...
            if (goalCount > 0)
            {
                for (int g = 0; g < goalCount; g++)
                {
                    var p = _g2w.GridToWorldCenter(r.goals[g]);
                    SetLabel(labelList[idx++], $"G{g}", new Vector3(p.x, _labelY, p.z));
                }
            }
        }

        private List<Vector3> BuildRepresentativeLinePoints(RouteDefinition r)
        {
            var pts = new List<Vector3>();

            int spawnCount = r.spawns?.Count ?? 0;
            int goalCount  = r.goals?.Count  ?? 0;

            // 代表線を引くには最低でもS0とG0が必要
            if (spawnCount == 0 || goalCount == 0)
                return pts;

            // S0
            {
                var s = _g2w.GridToWorldCenter(r.spawns[0]);
                pts.Add(new Vector3(s.x, _lineY, s.z));
            }

            // waypoints
            if (r.waypoints != null)
            {
                for (int i = 0; i < r.waypoints.Count; i++)
                {
                    var p = _g2w.GridToWorldCenter(r.waypoints[i]);
                    pts.Add(new Vector3(p.x, _lineY, p.z));
                }
            }

            // G0
            {
                var g = _g2w.GridToWorldCenter(r.goals[0]);
                pts.Add(new Vector3(g.x, _lineY, g.z));
            }

            return pts;
        }


        private LineRenderer GetOrCreateLine(string routeId)
        {
            if (_lines.TryGetValue(routeId, out var lr) && lr != null) return lr;

            var go = new GameObject($"RouteLine_{routeId}");
            go.transform.SetParent(_lineRoot, false);

            lr = go.AddComponent<LineRenderer>();
            lr.useWorldSpace = true;
            lr.material = _lineMaterial;
            lr.numCapVertices = 4;
            lr.numCornerVertices = 4;
            lr.alignment = LineAlignment.View; // カメラ向きで見やすく
            lr.widthMultiplier = _lineWidth;

            _lines[routeId] = lr;
            return lr;
        }

        private List<TextMesh> GetOrCreateLabels(string routeId)
        {
            if (_labels.TryGetValue(routeId, out var list) && list != null) return list;
            list = new List<TextMesh>();
            _labels[routeId] = list;
            return list;
        }

        private void EnsureLabelCount(List<TextMesh> list, int need, string routeId)
        {
            while (list.Count < need)
            {
                var go = new GameObject($"RouteLabel_{routeId}_{list.Count}");
                go.transform.SetParent(_labelRoot, false);

                var tm = go.AddComponent<TextMesh>();
                tm.font = _labelFont;
                tm.characterSize = _labelCharSize;
                tm.anchor = TextAnchor.MiddleCenter;
                tm.alignment = TextAlignment.Center;

                list.Add(tm);
            }

            // 余ったら非表示（DestroyでもOKだが、編集時に再利用できる）
            for (int i = 0; i < list.Count; i++)
                list[i].gameObject.SetActive(i < need);
        }

        private void SetLabel(TextMesh tm, string text, Vector3 basePos)
        {
            tm.text = text;
            tm.transform.position = new Vector3(basePos.x, _labelY, basePos.z);
            tm.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // 上から見る前提
        }

        private void CleanupUnused(IReadOnlyList<RouteDefinition> routes)
        {
            var alive = new HashSet<string>();
            for (int i = 0; i < routes.Count; i++)
                if (routes[i] != null && !string.IsNullOrWhiteSpace(routes[i].routeId))
                    alive.Add(routes[i].routeId);

            // lines
            var removeLines = new List<string>();
            foreach (var kv in _lines)
            {
                if (!alive.Contains(kv.Key))
                {
                    if (kv.Value != null) Destroy(kv.Value.gameObject);
                    removeLines.Add(kv.Key);
                }
            }

            for (int i = 0; i < removeLines.Count; i++) _lines.Remove(removeLines[i]);

            // labels
            var removeLabels = new List<string>();
            foreach (var kv in _labels)
            {
                if (!alive.Contains(kv.Key))
                {
                    if (kv.Value != null)
                    {
                        for (int i = 0; i < kv.Value.Count; i++)
                            if (kv.Value[i] != null)
                                Destroy(kv.Value[i].gameObject);
                    }

                    removeLabels.Add(kv.Key);
                }
            }

            for (int i = 0; i < removeLabels.Count; i++) _labels.Remove(removeLabels[i]);
        }

        private void ClearAll()
        {
            foreach (var kv in _lines)
                if (kv.Value != null)
                    Destroy(kv.Value.gameObject);
            _lines.Clear();

            foreach (var kv in _labels)
            {
                var list = kv.Value;
                if (list == null) continue;
                for (int i = 0; i < list.Count; i++)
                    if (list[i] != null)
                        Destroy(list[i].gameObject);
            }

            _labels.Clear();
        }
    }
}