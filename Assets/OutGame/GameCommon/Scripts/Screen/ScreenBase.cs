using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScreenBase : MonoBehaviour
{
    [SerializeField] protected GameObject _canvas;

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
