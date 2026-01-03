using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSelectScreen : ScreenBase
{
    public void Awake()
    {
        _canvas = this.gameObject;
    }

    public async override UniTask FadeOutScreen()
    {
        
    }

    public async override UniTask FadeInScreen()
    {

    }
}
