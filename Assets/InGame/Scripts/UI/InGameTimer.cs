using TMPro;
using UnityEngine;

public class InGameTimer : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Start()
    {
        InGameManager.Instance.OnTimeUpdated += TimerUpdate;
        text = GetComponent<TextMeshProUGUI>();
    }

    public void TimerUpdate(float time)
    {
        string minutes = ((int)time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        text.text = minutes + ":" + seconds;
    }
}
