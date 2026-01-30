using System.Collections.Generic;

namespace TD.Map
{
    /// <summary>
    /// Road上のみを歩ける前提で、到達可能性を調べる（BFS）。
    /// </summary>
    public static class RoadPathfinder
    {
        private static readonly (int dx, int dy)[] _dirs =
        {
            ( 1, 0),
            (-1, 0),
            ( 0, 1),
            ( 0,-1),
        };

        public static bool CanReach(GridMap map, Int2 start, Int2 goal)
        {
            if (!map.InBounds(start.x, start.y) || !map.InBounds(goal.x, goal.y)) return false;
            if (map.Get(start.x, start.y) != TileType.Road) return false;
            if (map.Get(goal.x, goal.y) != TileType.Road) return false;

            var visited = new bool[map.Width * map.Height];
            var q = new Queue<Int2>();
            q.Enqueue(start);
            visited[map.ToIndex(start.x, start.y)] = true;

            while (q.Count > 0)
            {
                var p = q.Dequeue();
                if (p.x == goal.x && p.y == goal.y) return true;

                for (int i = 0; i < _dirs.Length; i++)
                {
                    var nx = p.x + _dirs[i].dx;
                    var ny = p.y + _dirs[i].dy;
                    if (!map.InBounds(nx, ny)) continue;
                    if (map.Get(nx, ny) != TileType.Road) continue;

                    var idx = map.ToIndex(nx, ny);
                    if (visited[idx]) continue;

                    visited[idx] = true;
                    q.Enqueue(new Int2(nx, ny));
                }
            }

            return false;
        }
        
        static bool CanFollowRoute(GridMap map, Int2 spawn, List<Int2> waypoints, Int2 goal)
        {
            var current = spawn;

            foreach (var wp in waypoints)
            {
                if (!RoadPathfinder.CanReach(map, current, wp))
                    return false;
                current = wp;
            }

            return RoadPathfinder.CanReach(map, current, goal);
        }

    }
}