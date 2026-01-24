using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Result : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _fadePanel;
    [SerializeField] private Image _resultImage;
    [SerializeField] private Sprite[] _starImages = new Sprite[5];
    [SerializeField] private Image _starImage;
    private bool _isWin = true;

    public void SetIsWin(bool isWin) { _isWin = isWin; }

    private void Start()
    {
        Debug.Log("Fade");
        _fadePanel.color = new Color(0, 0, 0, 0);
        _resultImage.color = new Color(0, 0, 0, 0);
        _starImage.color = new Color(0, 0, 0, 0);
        _fadePanel.raycastTarget = true;
    }

    public void StartResult()
    {
        var tween1 = _fadePanel.DOColor(new Color(255, 255, 255, 150), 1f);
        var tween2 = _resultImage.DOColor(new Color(255, 255, 255, 255), 1f);
        tween1.onComplete = () => _starImage.color = new Color(255, 255, 255, 255);
    }

    public void SetResultScore(int maxEnemyNum, int lostEnemyNum)
    {
        if (lostEnemyNum <= 0)
        {
            _starImage.sprite = _starImages[4];
        }
        else
        {
            int starnum = 0;
            int tmp = lostEnemyNum;
            while ((tmp -= (maxEnemyNum / 5)) < 0)
            {
                starnum++;
            }
            _starImage.sprite = _starImages[starnum % 5];
        Debug.Log($"{starnum}");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SceneChanger.SceneChange("Home");
    }
}
