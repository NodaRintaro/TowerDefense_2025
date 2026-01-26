using UnityEngine;

namespace TD.View3D
{
    /// <summary>
    /// 3D(XZ)上にグリッド線を描画する（20x20など小規模向け）。
    /// LineRendererを(縦+横)本生成して表示する。
    /// </summary>
    public sealed class GridOverlay3D : MonoBehaviour
    {
        [Header("Grid Size")]
        [SerializeField] private int _width = 20;
        [SerializeField] private int _height = 20;

        [Header("Placement")]
        [SerializeField] private Vector3 _origin = Vector3.zero; // (0,0)の左下角
        [SerializeField] private float _tileSize = 1f;

        [Header("Visual")]
        [SerializeField] private float _y = 0.02f;               // 床より少し上
        [SerializeField] private float _lineWidth = 0.02f;
        [SerializeField] private Material _lineMaterial;

        [Header("Behavior")]
        [SerializeField] private bool _rebuildOnStart = true;

         public Transform _root;

        private void Awake()
        {
            _root = new GameObject("GridLines").transform;
            //_root.SetParent(transform, false);
        }

        private void Start()
        {
            if (_rebuildOnStart)
                Rebuild();
        }

        /// <summary>グリッド線を作り直す</summary>
        public void Rebuild()
        {
            Clear();

            if (_width <= 0 || _height <= 0) return;

            // 縦線：x = 0..width
            for (int x = 0; x <= _width; x++)
            {
                var p0 = _origin + new Vector3(x * _tileSize-0.5f, _y, -0.5f);
                var p1 = _origin + new Vector3(x * _tileSize-0.5f, _y, _height * _tileSize-0.5f);
                CreateLine($"V_{x}", p0, p1);
            }

            // 横線：y = 0..height（※グリッド座標のyはZ方向）
            for (int y = 0; y <= _height; y++)
            {
                var p0 = _origin + new Vector3(0f-0.5f, _y, y * _tileSize-0.5f);
                var p1 = _origin + new Vector3(_width * _tileSize-0.5f, _y, y * _tileSize-0.5f);
                CreateLine($"H_{y}", p0, p1);
            }
        }

        public void SetSize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void SetOriginAndTileSize(Vector3 origin, float tileSize)
        {
            _origin = new Vector3(origin.x, 0.05f, origin.z);
            _tileSize = tileSize;
        }

        private void CreateLine(string name, Vector3 p0, Vector3 p1)
        {
            var go = new GameObject(name);
            go.transform.SetParent(_root, false);

            var lr = go.AddComponent<LineRenderer>();
            lr.useWorldSpace = true;
            lr.positionCount = 2;
            lr.SetPosition(0, p0);
            lr.SetPosition(1, p1);

            lr.material = _lineMaterial;
            lr.widthMultiplier = _lineWidth;

            lr.numCapVertices = 0;
            lr.numCornerVertices = 0;
            lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lr.receiveShadows = false;
        }

        private void Clear()
        {
            if (_root == null) return;

            for (int i = _root.childCount - 1; i >= 0; i--)
                Destroy(_root.GetChild(i).gameObject);
        }
    }
}
