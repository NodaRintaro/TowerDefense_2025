using System.Collections.Generic;
using TD.Map;

namespace TD.Game
{
    public static class MapValidator
    {
        public static bool ValidateAllRoutes(GridMap map, List<RouteDefinition> routes, out string error)
        {
            error = null;

            if (map == null)
            {
                error = "Map is null.";
                return false;
            }

            if (routes == null || routes.Count == 0)
            {
                error = "Routeがありません。";
                return false;
            }

            var idSet = new HashSet<string>();

            for (int i = 0; i < routes.Count; i++)
            {
                var r = routes[i];
                if (r == null)
                {
                    error = $"Route[{i}] is null.";
                    return false;
                }

                if (r.spawns == null || r.spawns.Count == 0)
                {
                    error = $"Route({r.routeId}) spawnがありません";
                    return false;
                }

                if (r.goals == null || r.goals.Count == 0)
                {
                    error = $"Route({r.routeId}) goalがありません";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(r.routeId))
                {
                    error = $"Route[{i}] の routeId が空です。";
                    return false;
                }

                if (!idSet.Add(r.routeId))
                {
                    error = $"routeId が重複しています: {r.routeId}";
                    return false;
                }

                // Road上チェック：spawns/goalsをループ
                for (int s = 0; s < r.spawns.Count; s++)
                    if (!IsRoad(map, r.spawns[s]))
                    {
                        error = $"Route({r.routeId}) spawn[{s}] はRoad上に";
                        return false;
                    }

                for (int g = 0; g < r.goals.Count; g++)
                    if (!IsRoad(map, r.goals[g]))
                    {
                        error = $"Route({r.routeId}) goal[{g}] はRoad上に";
                        return false;
                    }

                if (r.waypoints != null)
                {
                    for (int w = 0; w < r.waypoints.Count; w++)
                    {
                        if (!IsRoad(map, r.waypoints[w]))
                        {
                            error = $"Route({r.routeId}) waypoint[{w}] はRoad上に置いてください。";
                            return false;
                        }
                    }
                }

                // 接続チェック：各spawnが、waypoints経由で、どれかのgoalへ到達できる
                for (int s = 0; s < r.spawns.Count; s++)
                {
                    bool any = false;
                    for (int g = 0; g < r.goals.Count; g++)
                    {
                        if (CanFollowRoute(map, r.routeId, r.spawns[s], r.waypoints, r.goals[g], out error))
                        {
                            any = true;
                            break;
                        }
                    }

                    if (!any)
                    {
                        error = $"Route({r.routeId}) spawn[{s}]から到達可能なgoalがありません";
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool IsRoad(GridMap map, Int2 p)
        {
            if (!map.InBounds(p.x, p.y)) return false;
            return map.Get(p.x, p.y) == TileType.Road;
        }

        private static bool CanFollowRoute(GridMap map, string routeId, Int2 start, List<Int2> waypoints, Int2 goal,
            out string error)
        {
            error = null;

            Int2 current = start;

            if (waypoints != null)
            {
                for (int i = 0; i < waypoints.Count; i++)
                {
                    var next = waypoints[i];
                    if (!TD.Map.RoadPathfinder.CanReach(map, current, next))
                    {
                        error = $"Route(で接続不可: {ToStr(current)} -> waypoint[{i}] {ToStr(next)}";
                        return false;
                    }

                    current = next;
                }
            }

            if (!TD.Map.RoadPathfinder.CanReach(map, current, goal))
            {
                error = $"Route({routeId}) で接続不可: {ToStr(current)} -> goal {ToStr(goal)}";
                return false;
            }

            return true;
        }

        private static string ToStr(Int2 p) => $"({p.x},{p.y})";
    }
}