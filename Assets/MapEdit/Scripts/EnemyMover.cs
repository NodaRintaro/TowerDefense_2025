using System.Collections.Generic;
using UnityEngine;

namespace TD.Game
{
    /// <summary>
    /// ルートの点列に沿って移動する。3D(XZ)前提。
    /// </summary>
    public sealed class EnemyMover : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 2.5f;
        [SerializeField] private float _arriveDistance = 0.1f;
        
        private IReadOnlyList<Vector3> _points;
        private RouteRuntime _route;
        private int _index;

        
        public void SetPoints(IReadOnlyList<Vector3> points)
        {
            _points = points;
            _index = 0;

            if (_points != null && _points.Count > 0)
            {
                var p = _points[0];
                transform.position = new Vector3(p.x, transform.position.y, p.z);
            }
        }
        
        private void Update()
        {
            if (_points == null) return;
            if (_index >= _points.Count) return;

            var target = _points[_index];
            var pos = transform.position;

            // yは現在のまま（地形高さがあるなら別途サンプル）
            var targetPos = new Vector3(target.x, pos.y);

            var dir = (targetPos - pos);
            var dist = dir.magnitude;

            if (dist <= _arriveDistance)
            {
                _index++;
                return;
            }

            var step = _moveSpeed * Time.deltaTime;
            transform.position = pos + dir.normalized * Mathf.Min(step, dist);

            // 向き
            if (dir.sqrMagnitude > 0.0001f)
            {
                var look = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, look, 12f * Time.deltaTime);
            }
        }
    }
}