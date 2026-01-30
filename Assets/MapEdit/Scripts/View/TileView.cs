using UnityEngine;

namespace TD.View3D
{
    /// <summary>
    /// 1タイルの見た目（3Dオブジェクト）を管理する。
    /// </summary>
    public sealed class TileView : MonoBehaviour
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public void Initialize(int x, int y)
        {
            X = x;
            Y = y;
            name = $"Tile_{x}_{y}";
        }
    }
}