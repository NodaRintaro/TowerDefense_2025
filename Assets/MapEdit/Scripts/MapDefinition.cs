using System;
using System.Collections.Generic;

namespace TD.Map
{
    [Serializable]
    public sealed class MapDefinition
    {
        public int mapId;
        public string mapName;
        public int version = 4;

        public int width;
        public int height;

        /// <summary>width*height の一次元配列（y*width + x）</summary>
        public int[] tiles;

        /// <summary>複数ルート</summary>
        public List<RouteDefinition> routes = new();
    }

    [Serializable]
    public sealed class RouteDefinition
    {
        /// <summary>ユニークID（例: "A", "B", "route_01"）</summary>
        public string routeId;

        /// <summary>SpawnはRoad扱い、1つに固定（必要なら複数に拡張可）</summary>
        public List<Int2> spawns = new();
        

        /// <summary>GoalはRoad扱い、1つに固定</summary>
        public List<Int2> goals  = new();

        /// <summary>経由地点（順番通り）</summary>
        public List<Int2> waypoints = new();
    }

    [Serializable]
    public struct Int2
    {
        public int x;
        public int y;
        public Int2(int x, int y) { this.x = x; this.y = y; }
    }
}