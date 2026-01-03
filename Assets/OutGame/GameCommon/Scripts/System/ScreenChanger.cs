using Cysharp.Threading.Tasks;
using UnityEngine;

public class ScreenChanger
{
    private ScreenBase _currentScreen;

    public async UniTask ChangeScreen(ScreenBase screen)
    {
        if (_currentScreen != screen && _currentScreen != null)
        {
            await _currentScreen.FadeOutScreen();
            _currentScreen = screen;
        }

        await screen.FadeInScreen();
    }
}
