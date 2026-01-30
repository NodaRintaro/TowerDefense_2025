using System.Collections.Generic;
using UnityEngine;
using TD.Map;

namespace TD.Game
{
    public sealed class RouteRuntime
    {
        public string RouteId { get; }

        private readonly RouteDefinition _def;
        private readonly GridToWorld _g2w;
        
        public RouteDefinition Definition => _def;
        public GridToWorld GridToWorld => _g2w;

        public RouteRuntime(RouteDefinition def, GridToWorld g2w)
        {
            _def = def;
            _g2w = g2w;
            RouteId = def.routeId;
        }

        public int SpawnCount => _def.spawns?.Count ?? 0;
        public int GoalCount  => _def.goals?.Count ?? 0;

        public IReadOnlyList<Vector3> BuildPoints(int spawnIndex, int goalIndex)
        {
            var pts = new List<Vector3>();

            var spawn = _def.spawns[spawnIndex];
            pts.Add(_g2w.GridToWorldCenter(spawn));

            if (_def.waypoints != null)
            {
                for (int i = 0; i < _def.waypoints.Count; i++)
                    pts.Add(_g2w.GridToWorldCenter(_def.waypoints[i]));
            }

            var goal = _def.goals[goalIndex];
            pts.Add(_g2w.GridToWorldCenter(goal));

            return pts;
        }
    }
}