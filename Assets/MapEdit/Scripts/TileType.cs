namespace TD.Map
{
    /// <summary>
    /// タイル種類。Empty(建設可), Road(建設不可), Block(通行不可)
    /// </summary>
    public enum TileType : int
    {
        Empty = 0,
        Road  = 1,
        HighPlatform = 2,
        Block = 3,
    }
}