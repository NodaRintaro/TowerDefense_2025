using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TitleLogo : MonoBehaviour
{
    [SerializeField] private TitleScreen _titleScreen;

    [SerializeField] private GameObject _titleLogo;

    [SerializeField] private GameObject _gameStartText;

    [SerializeField] private float _centerPos = 0;

    [SerializeField] private float _duration = 1.5f;

    public async void Start()
    {
        _gameStartText.SetActive(false);
        await SlideInAnimation.SlideInGameObjectFromTop(_titleLogo, _centerPos, _duration);
        _titleScreen.IsSceneChange(true);
        _gameStartText.SetActive(true);
    }
}
