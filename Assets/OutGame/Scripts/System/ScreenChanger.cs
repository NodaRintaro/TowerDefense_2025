using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ScreenChanger
{
    private List<GameObject> _screenCanvasList = new();

    public List<GameObject> ScreenCanvaList => _screenCanvasList;

    [Inject]
    public ScreenChanger() { }

    /// <summary> 表示画面の切り替え </summary>
    public void ScreenChange(string screenName)
    {
        foreach(GameObject canvas in _screenCanvasList)
        {
            if (canvas.name == screenName)
            {
                canvas.SetActive(true);
            }
            else if(canvas == canvas.activeSelf)
            {
                canvas.SetActive(false);
            }
        }
    }
}
