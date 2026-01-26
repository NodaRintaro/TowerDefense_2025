using UnityEngine;

namespace TD.Map
{
    /// <summary>
    /// 3D(XZ)上のグリッド変換。タイルサイズ1を想定。
    /// origin は(0,0)タイルの中心位置。
    /// </summary>
    public sealed class GridToWorld
    {
        public Vector3 Origin { get; }
        public float TileSize { get; }

        public GridToWorld(Vector3 origin, float tileSize = 1f)
        {
            Origin = origin;
            TileSize = tileSize;
        }

        public Vector3 GridToWorldCenter(Int2 p)
        {
            return Origin + new Vector3(p.x * TileSize, 0f, p.y * TileSize);
        }

        public bool TryWorldToGrid(Vector3 world, out Int2 p)
        {
            var local = world - Origin;
            int x = Mathf.RoundToInt(local.x / TileSize);
            int y = Mathf.RoundToInt(local.z / TileSize);
            p = new Int2(x, y);
            return true;
        }
    }
}