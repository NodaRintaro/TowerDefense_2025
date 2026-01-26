using System.Collections.Generic;
using TD.Map;

namespace TD.Game
{
    public sealed class RouteRepository
    {
        private readonly Dictionary<string, RouteRuntime> _routes = new();

        public RouteRepository(MapDefinition def, GridToWorld g2w)
        {
            if (def.routes == null) return;

            for (int i = 0; i < def.routes.Count; i++)
            {
                var r = def.routes[i];
                if (string.IsNullOrWhiteSpace(r.routeId)) continue;

                _routes[r.routeId] = new RouteRuntime(r, g2w);
            }
        }

        public bool TryGet(string routeId, out RouteRuntime route)
            => _routes.TryGetValue(routeId, out route);
    }
}