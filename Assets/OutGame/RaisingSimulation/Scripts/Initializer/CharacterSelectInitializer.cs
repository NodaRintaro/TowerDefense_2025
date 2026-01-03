using UnityEngine;
using VContainer;
using VContainer.Unity;

/// <summary>
/// キャラクター選択画面ですべてのマスターデータのロード終了後の処理を行う
/// </summary>
public class CharacterSelectInitializer : IPostStartable
{
    [Inject]
    public CharacterSelectInitializer(){ }

    public void PostStart()
    {
        Debug.Log("ゲームロジック開始");
    }
}
