using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class ScreenBase : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;

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
