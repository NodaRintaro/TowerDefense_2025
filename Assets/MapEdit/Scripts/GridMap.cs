using System;
using System.Collections.Generic;

namespace TD.Map
{
    /// <summary>
    /// 実行時のグリッド。MapDefinition の tiles を安全に扱う薄いラッパー。
    /// </summary>
    public sealed class GridMap
    {
        public int Width { get; }
        public int Height { get; }

        private readonly TileType[] _tiles;

        public GridMap(int width, int height, TileType fill = TileType.Empty)
        {
            if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException();
            Width = width;
            Height = height;

            _tiles = new TileType[width * height];
            for (int i = 0; i < _tiles.Length; i++) _tiles[i] = fill;
        }

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

        public int ToIndex(int x, int y) => y * Width + x;

        public TileType Get(int x, int y)
        {
            if (!InBounds(x, y)) throw new ArgumentOutOfRangeException();
            return _tiles[ToIndex(x, y)];
        }

        public void Set(int x, int y, TileType type)
        {
            if (!InBounds(x, y)) throw new ArgumentOutOfRangeException();
            _tiles[ToIndex(x, y)] = type;
        }

        public TileType[] GetRawTilesCopy()
        {
            var copy = new TileType[_tiles.Length];
            Array.Copy(_tiles, copy, _tiles.Length);
            return copy;
        }

        public static GridMap FromDefinition(MapDefinition def)
        {
            if (def == null) throw new ArgumentNullException(nameof(def));
            var map = new GridMap(def.width, def.height, TileType.Empty);

            if (def.tiles != null && def.tiles.Length == def.width * def.height)
            {
                for (int i = 0; i < def.tiles.Length; i++)
                    map._tiles[i] = (TileType)def.tiles[i];
            }

            return map;
        }

        public MapDefinition ToDefinition(int mapId, int version, List<RouteDefinition> routes)
        {
            var def = new MapDefinition
            {
                mapId = mapId,
                version = version,
                width = Width,
                height = Height,
                tiles = new int[Width * Height],
                routes = routes ?? new List<RouteDefinition>(),
            };

            for (int i = 0; i < _tiles.Length; i++)
                def.tiles[i] = (int)_tiles[i];

            return def;
        }
    }
}
