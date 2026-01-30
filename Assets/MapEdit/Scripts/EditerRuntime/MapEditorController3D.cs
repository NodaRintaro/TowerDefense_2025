using System.Collections.Generic;
using UnityEngine;
using TD.Map;
using TD.Game;

namespace TD.EditorRuntime
{
    /// <summary>
    /// 3D向けのゲーム内マップエディタ中核（複数ルート対応）
    /// </summary>
    public sealed class MapEditorController3D : MonoBehaviour
    {
        [Header("Map")] [SerializeField] private int _mapId = 0;
        [SerializeField] private string _mapName = "New Map";
        [SerializeField] private int _width = 20;
        [SerializeField] private int _height = 12;

        [Header("3D Grid")] [SerializeField] private Vector3 _origin = Vector3.zero;
        [SerializeField] private float _tileSize = 1f;

        [Header("Editing")] [SerializeField] private TileType _brush = TileType.Empty;
        [SerializeField] private EditTool _tool = EditTool.Paint;

        [Header("Active Route")] [SerializeField]
        private int _activeRouteIndex = 0;

        private GridMap _map;
        private GridToWorld _g2w;

        private readonly List<MapDefinition> _maps = new();
        private readonly List<RouteDefinition> _routes = new();

        public enum EditTool { Paint, AddSpawn, AddGoal, AddWaypoint, RemovePoint }

        public GridMap Map => _map;
        public IReadOnlyList<RouteDefinition> Routes => _routes;
        public IReadOnlyList<MapDefinition> Maps => _maps;

        public event System.Action OnMapChanged;
        public event System.Action OnRoutesChanged;


        public int ActiveRouteIndex => _activeRouteIndex;
        public RouteDefinition ActiveRoute => GetActiveRoute();

        public int MapId
        {
            get => _mapId;
            set => _mapId = value;
        }
        
        public string MapName
        {
            get => _mapName;
            set => _mapName = value;
        }

        private void Awake()
        {
            _g2w = new GridToWorld(_origin, _tileSize);
            CreateNew(_width, _height);
            EnsureRouteExists();
        }

        public bool TryWorldToGrid(Vector3 world, out Int2 p) => _g2w.TryWorldToGrid(world, out p);

        public void CreateNew(int width, int height)
        {
            _map = new GridMap(width, height, TileType.Road);
            _routes.Clear();
            _activeRouteIndex = 0;
        }

        public void SetBrush(TileType brush) => _brush = brush;
        public void SetTool(EditTool tool) => _tool = tool;

        public void AddRoute(string routeId)
        {
            _routes.Add(new RouteDefinition
            {
                routeId = routeId,
                spawns = new System.Collections.Generic.List<Int2>(),
                goals = new System.Collections.Generic.List<Int2>(),
                waypoints = new List<Int2>()
            });
            _activeRouteIndex = _routes.Count - 1;
        }

        public void SetActiveRouteIndex(int index)
        {
            _activeRouteIndex = Mathf.Clamp(index, 0, _routes.Count - 1);
            OnRoutesChanged?.Invoke();
        }

        private void EnsureRouteExists()
        {
            if (_routes.Count == 0)
                AddRoute("A");
        }

        private RouteDefinition GetActiveRoute()
        {
            EnsureRouteExists();
            _activeRouteIndex = Mathf.Clamp(_activeRouteIndex, 0, _routes.Count - 1);
            return _routes[_activeRouteIndex];
        }

        public void ApplyAt(int x, int y)
        {
            if (_map == null || !_map.InBounds(x, y)) return;

            switch (_tool)
            {
                case EditTool.Paint:
                    _map.Set(x, y, _brush);
                    OnMapChanged?.Invoke();
                    break;

                case EditTool.AddSpawn:
                    {
                        if (_map.Get(x, y) != TileType.Road) return;
                        var r = GetActiveRoute();
                        r.spawns ??= new List<Int2>();
                        AddUnique(r.spawns, x, y);
                        OnRoutesChanged?.Invoke();
                        break;
                    }

                case EditTool.AddGoal:
                    {
                        if (_map.Get(x, y) != TileType.Road) return;
                        var r = GetActiveRoute();
                        r.goals ??= new List<Int2>();
                        AddUnique(r.goals, x, y);
                        OnRoutesChanged?.Invoke();
                        break;
                    }
                case EditTool.AddWaypoint:
                    {
                        if (_map.Get(x, y) != TileType.Road) return;
                        var r = GetActiveRoute();
                        r.waypoints ??= new List<Int2>();
                        r.waypoints.Add(new Int2(x, y));
                        OnRoutesChanged?.Invoke();
                        break;
                    }

                case EditTool.RemovePoint:
                    {
                        var r = GetActiveRoute();
                        RemoveAt(r.spawns, x, y);
                        RemoveAt(r.goals, x, y);
                        RemoveAt(r.waypoints, x, y);
                        OnRoutesChanged?.Invoke();
                        break;
                    }
            }
        }

        public bool TrySaveUser(out string error)
        {
            error = null;

            if (!TD.Game.MapValidator.ValidateAllRoutes(_map, _routes, out error))
                return false;

            var def = new MapDefinition
            {
                mapId = _mapId,
                mapName = _mapName,
                version = 3,
                width = _map.Width,
                height = _map.Height,
                tiles = new int[_map.Width * _map.Height],
                routes = new List<RouteDefinition>(_routes),
            };

            // tiles
            var raw = _map.GetRawTilesCopy();
            for (int i = 0; i < raw.Length; i++) def.tiles[i] = (int)raw[i];

            TD.Map.MapSerializer.SaveUserByStageName(def);
            return true;
        }
        
        public bool TryLoad(int mapId)
        {

            return Load(mapId);
        }
        
        public bool Load(int mapId)
        {
            _mapId = mapId;
            
            if (!MapSerializer.TryLoadUserById(mapId,out MapDefinition def))
                return false;
            _mapName = def.mapName;

            _map = GridMap.FromDefinition(def);
            _routes.Clear();
            _routes.AddRange(def.routes ?? new List<RouteDefinition>());

            _activeRouteIndex = Mathf.Clamp(_activeRouteIndex, 0, _routes.Count - 1);

            OnMapChanged?.Invoke();
            OnRoutesChanged?.Invoke();
            return true;
        }

        private static void AddUnique(List<Int2> list, int x, int y)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].x == x && list[i].y == y)
                    return;
            list.Add(new Int2(x, y));
        }

        private static void RemoveAt(List<Int2> list, int x, int y)
        {
            if (list == null) return;
            for (int i = list.Count - 1; i >= 0; i--)
                if (list[i].x == x && list[i].y == y)
                    list.RemoveAt(i);
        }

        

        // ルート追加（routeId自動採番でもOK）
        public void AddRouteAuto()
        {
            var id = $"R{_routes.Count}";
            AddRoute(id); // 既存のAddRoute(string)
            OnRoutesChanged?.Invoke();
        }
        
        public void RemoveActiveRoute()
        {
            if (_routes.Count == 0) return;
            _routes.RemoveAt(_activeRouteIndex);
            _activeRouteIndex = Mathf.Clamp(_activeRouteIndex, 0, _routes.Count - 1);
            OnRoutesChanged?.Invoke();
        }

        // Waypoint並び替え
        public void MoveWaypoint(int from, int to)
        {
            var r = GetActiveRoute();
            if (r.waypoints == null) return;
            if (from < 0 || from >= r.waypoints.Count) return;
            if (to < 0 || to >= r.waypoints.Count) return;

            var item = r.waypoints[from];
            r.waypoints.RemoveAt(from);
            r.waypoints.Insert(to, item);
            OnRoutesChanged?.Invoke();
        }
    }
}