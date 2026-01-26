using System;
using System.Collections.Generic;

namespace TD.Map
{
    [Serializable]
    public sealed class MapIndex
    {
        public int version = 1;
        public List<MapIndexEntry> entries = new();
    }

    [Serializable]
    public sealed class MapIndexEntry
    {
        public int mapId;
        public string stageName;
        public string fileName; // 例: "Stage01__a1b2c3.json"
        public long updatedAtUnix;
    }
}