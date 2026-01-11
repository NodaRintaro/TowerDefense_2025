using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScreenBase : MonoBehaviour
{
    protected GameObject _canvas;

    public void Awake()
    {
        _canvas = this.gameObject;
    }

    public virtual UniTask Init()
    {
        return UniTask.CompletedTask;
    }

    public virtual UniTask FadeOutScreen()
    {
        return UniTask.CompletedTask;
    }

    public virtual UniTask FadeInScreen()
    {
        return UniTask.CompletedTask;
    }
}
