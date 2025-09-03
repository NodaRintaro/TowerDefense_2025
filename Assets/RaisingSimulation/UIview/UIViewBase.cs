using UnityEngine;

[System.Serializable]
public class UIViewBase
{
    [SerializeField, Header("View‚Ìƒ^ƒCƒv")]
    private ScreenType viewType = default;

    [SerializeField, Header("CanvasObject")]
    private GameObject _viewCanvasObj = null;

    public ScreenType ViewType => viewType;

    public GameObject ViewCanvasObj => _viewCanvasObj;
}
