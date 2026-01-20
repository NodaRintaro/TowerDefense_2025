public class StageDataLoader
{
    //タワーディフェンスで使うステージのデータ
    private static StageData _currentUseStageData;
    
    public static StageData CurrentUseDeck => _currentUseStageData;

    /// <summary> デッキのデータをセットする </summary>
    public static void SetStage(StageData characterStageData)
    {
        _currentUseStageData = characterStageData;
    }

    /// <summary> デッキのデータを取得する </summary>
    public static StageData GetState()
    {
        return _currentUseStageData;
    }
}