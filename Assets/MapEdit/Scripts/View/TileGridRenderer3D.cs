using UnityEngine;
using TD.Map;

namespace TD.View3D
{
    /// <summary>
    /// 20x20程度を想定した、1マス=1GameObjectのタイル描画。
    /// MapEditorController3D の GridMap を見て、見た目を生成/更新する。
    /// </summary>
    public sealed class TileGridRenderer3D : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private TD.EditorRuntime.MapEditorController3D _controller;

        [Header("Tile Prefabs")]
        [SerializeField] private TileView _emptyPrefab;
        [SerializeField] private TileView _roadPrefab;
        [SerializeField] private TileView _highPlatformPrefab;
        [SerializeField] private TileView _blockPrefab;

        [Header("Placement")]
        [SerializeField] private Transform _tileRoot;
        [SerializeField] private Vector3 _origin = Vector3.zero;
        [SerializeField] private float _tileSize = 1f;
        [SerializeField] private float _yOffset = 0f;

        private TileView[,] _views;
        private TileType[,] _currentTypes; // 変更差分更新用

        private void Awake()
        {
            if (_tileRoot == null) _tileRoot = this.transform;
        }

        private void Start()
        {
            RebuildAll();
        }

        /// <summary>全タイルを作り直す（マップロード/新規作成時に呼ぶ）。</summary>
        public void RebuildAll()
        {
            if (_controller == null || _controller.Map == null) return;

            ClearChildren(_tileRoot);

            var map = _controller.Map;
            _views = new TileView[map.Width, map.Height];
            _currentTypes = new TileType[map.Width, map.Height];

            for (int y = 0; y < map.Height; y++)
            for (int x = 0; x < map.Width; x++)
            {
                var type = map.Get(x, y);
                _views[x, y] = CreateTileView(x, y, type);
                _currentTypes[x, y] = type;
            }
        }

        /// <summary>
        /// 変更があった場所だけ更新したい時に呼ぶ（毎フレームでも軽い）。
        /// </summary>
        public void RefreshDiff()
        {
            if (_controller == null || _controller.Map == null) return;
            if (_views == null) { RebuildAll(); return; }

            var map = _controller.Map;
            if (map.Width != _views.GetLength(0) || map.Height != _views.GetLength(1))
            {
                RebuildAll();
                return;
            }

            for (int y = 0; y < map.Height; y++)
            for (int x = 0; x < map.Width; x++)
            {
                var type = map.Get(x, y);
                if (_currentTypes[x, y] == type) continue;

                // 作り直し
                if (_views[x, y] != null) Destroy(_views[x, y].gameObject);
                _views[x, y] = CreateTileView(x, y, type);
                _currentTypes[x, y] = type;
            }
        }

        private TileView CreateTileView(int x, int y, TileType type)
        {
            var prefab = type switch
            {
                TileType.Empty => _emptyPrefab,
                TileType.Road => _roadPrefab,
                TileType.Block => _blockPrefab,
                TileType.HighPlatform => _highPlatformPrefab,
                _ => _emptyPrefab
            };

            var pos = _origin + new Vector3(x * _tileSize, _yOffset, y * _tileSize);
            var view = Instantiate(prefab, pos, Quaternion.identity, _tileRoot);
            view.Initialize(x, y);
            return view;
        }

        private static void ClearChildren(Transform root)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
                Destroy(root.GetChild(i).gameObject);
        }
    }
}
