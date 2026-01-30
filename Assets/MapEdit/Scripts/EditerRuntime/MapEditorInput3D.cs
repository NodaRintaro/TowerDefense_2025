using UnityEngine;
using TD.Map;

namespace TD.EditorRuntime
{
    public sealed class MapEditorInput3D : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private MapEditorController3D _controller;
        private TD.Map.Int2? _lastCell;
        private void Reset()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (TryGetGridUnderMouse(out var p))
                {
                    // 同じセルならスキップ
                    if (_lastCell.HasValue && _lastCell.Value.x == p.x && _lastCell.Value.y == p.y)
                        return;

                    _lastCell = p;
                    _controller.ApplyAt(p.x, p.y);
                }
            }
            else
            {
                _lastCell = null; // ボタン離したらリセット
            }
        }

        private bool TryGetGridUnderMouse(out Int2 p)
        {
            p = default;

            if (_camera == null) return false;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 999f, _groundMask))
                return false;
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 0.5f);
            return _controller.TryWorldToGrid(hit.point, out p);
        }
    }
}