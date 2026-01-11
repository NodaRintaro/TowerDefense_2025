using System;
using UnityEngine;


/// <summary>
/// 全てのデータリポジトリのロードが完了したことを通知するクラス
/// </summary>
public class DataLoadCompleteNotifier : MonoBehaviour
{
    public event Action OnDataLoadComplete;

    public void NotifyDataLoadComplete()
    {
        OnDataLoadComplete?.Invoke();
        Debug.Log("全てのデータリポジトリのロードが完了しました。");
    }
}
