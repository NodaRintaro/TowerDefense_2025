using UnityEngine;

namespace TD.View3D
{
    /// <summary>
    /// MapEditorController3D の変更イベントを受けて、3D描画を更新する。
    /// </summary>
    public sealed class MapViewUpdater : MonoBehaviour
    {
        [SerializeField] private TD.EditorRuntime.MapEditorController3D _controller;
        [SerializeField] private TileGridRenderer3D _tileRenderer;
        [SerializeField] private RouteVisualizer3D _routeVisualizer;
        [SerializeField] private TD.View3D.GridOverlay3D _grid;
        
        
        private void OnEnable()
        {
            if (_controller == null) return;

            _controller.OnMapChanged += HandleMapChanged;
            _controller.OnRoutesChanged += HandleRoutesChanged;
        }

        private void OnDisable()
        {
            if (_controller == null) return;

            _controller.OnMapChanged -= HandleMapChanged;
            _controller.OnRoutesChanged -= HandleRoutesChanged;
        }

        private void Start()
        {
            _tileRenderer?.RebuildAll();
            _routeVisualizer?.RebuildAll();
            
            _grid.SetSize(_controller.Map.Width, _controller.Map.Height);
            // Controllerのorigin/tileSizeを公開しているならそこから
            //_grid.SetOriginAndTileSize(new Vector3(0,0,0), 1f);
            _grid.Rebuild();
        }

        private void HandleMapChanged()
        {
            // 20x20なら差分更新で十分
            _tileRenderer?.RefreshDiff();
            // タイル変更で「Road→Block」になったらルートが壊れる可能性があるので、ルート表示も更新
            _routeVisualizer?.RefreshAll();
        }

        private void HandleRoutesChanged()
        {
            _routeVisualizer?.RefreshAll();
        }
    }
}