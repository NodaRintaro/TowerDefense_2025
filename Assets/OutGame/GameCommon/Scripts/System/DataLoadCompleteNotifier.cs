using System;
using UnityEngine;


/// <summary>
/// 全てのデータリポジトリのロードが完了したことを通知するクラス
/// </summary>
public class DataLoadCompleteNotifier : MonoBehaviour
{
    private bool _isDataLoadComplete = false;
    public bool IsDataLoadComplete => _isDataLoadComplete;

    public event Action OnDataLoadComplete;

    public void NotifyDataLoadComplete()
    {
        _isDataLoadComplete = true;
        OnDataLoadComplete?.Invoke();
        Debug.Log("全てのデータリポジトリのロードが完了しました。");
    }
}
