using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// RectTransform をマウス位置基準でズームし、ドラッグで移動できるようにする。
/// 可能な限り「入力があった時だけ」計算するよう最適化。
/// </summary>
[DisallowMultipleComponent]
public class RectTransformZoom : MonoBehaviour, IDragHandler
{
    [Header("References")]
    [SerializeField] private Canvas _uiCanvas;
    [SerializeField] private RectTransform _viewport; // 画面(見える範囲)となるRectTransform（ScrollViewのViewport等）

    [Header("Zoom")]
    [SerializeField] private float _zoomSpeed = 1f;
    [SerializeField] private float _minZoomRate = 1f;
    [SerializeField] private float _maxZoomRate = 10f;

    private RectTransform _content;
    private CanvasScaler _canvasScaler;
    private Camera _targetCamera;


    // Drag delta補正用（CanvasScalerが ScaleWithScreenSize の時のみ）
    private bool _shouldScaleDragMove;
    private float _dragMoveScale = 1f;

    private float CurrentZoomScale => _content.localScale.x;

    private void Awake()
    {
        _content = GetComponent<RectTransform>();
        _canvasScaler = _uiCanvas != null ? _uiCanvas.GetComponent<CanvasScaler>() : null;

        CacheCanvasRelated();
        CacheDragScale();
    }

    private void OnEnable()
    {
        CacheCanvasRelated();
        CacheDragScale();
    }

    private void OnRectTransformDimensionsChange()
    {
        // 画面サイズやCanvasの変化に追従（毎フレームやらない）
        CacheCanvasRelated();
        CacheDragScale();
        ClampContentToViewport();
    }

    private void Update()
    {
        // 入力がないフレームは何もしない（軽量化ポイント）
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Approximately(scroll, 0f)) return;

        ScrollToZoom(Input.mousePosition, scroll);
    }

    /// <summary>
    /// マウス位置を基準にズームし、ズーム前後でマウス位置の見え方がズレないように補正する。
    /// </summary>
    private void ScrollToZoom(Vector2 mousePosition, float scroll)
    {
        // ズーム前のローカル座標
        if (!TryGetLocalPoint(mousePosition, out var beforeLocal)) return;

        float afterScale = CurrentZoomScale + scroll * _zoomSpeed;
        afterScale = Mathf.Clamp(afterScale, _minZoomRate, _maxZoomRate);

        DoZoom(afterScale);

        // ズーム後のローカル座標
        if (!TryGetLocalPoint(mousePosition, out var afterLocal)) return;

        Vector2 diff = afterLocal - beforeLocal;

        // anchoredPosition補正（見た目上、マウス下の位置が動かないようにする）
        _content.anchoredPosition += diff * afterScale;

        ClampContentToViewport();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.delta;

        if (_shouldScaleDragMove)
            delta *= _dragMoveScale;

        _content.anchoredPosition += delta;

        ClampContentToViewport();
    }

    private void DoZoom(float zoomScale)
    {
        _content.localScale = Vector3.one * zoomScale;
    }

    private void CacheCanvasRelated()
    {
        if (_uiCanvas == null) return;

        _targetCamera = _uiCanvas.renderMode switch
        {
            RenderMode.ScreenSpaceCamera => _uiCanvas.worldCamera,
            RenderMode.ScreenSpaceOverlay => null,
            _ => null
        };
    }

    private void CacheDragScale()
    {
        _shouldScaleDragMove =
            _canvasScaler != null &&
            _canvasScaler.isActiveAndEnabled &&
            _canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize;

        if (_shouldScaleDragMove)
        {
            // 参照解像度基準で eventData.delta を補正
            // ※横基準。縦基準が良ければ referenceResolution.y / Screen.height に
            _dragMoveScale = _canvasScaler.referenceResolution.x / Screen.width;
        }
        else
        {
            _dragMoveScale = 1f;
        }
    }

    private bool TryGetLocalPoint(Vector2 screenPos, out Vector2 localPos)
    {
        if (_content == null)
        {
            localPos = default;
            return false;
        }

        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _content, screenPos, _targetCamera, out localPos);
    }

    /// <summary>
    /// Content が Viewport からはみ出さないように anchoredPosition を Clamp する。
    /// 固定値(960/540/2500)を使わず、Rectサイズから算出するので運用が軽い。
    /// </summary>
    private void ClampContentToViewport()
    {
        if (_viewport == null) return;
        if(_content == null) return;

        // pivot を考慮しつつ、Viewport内に収まる anchoredPosition の範囲を計算
        Vector2 viewportSize = _viewport.rect.size;
        Vector2 contentSize = _content.rect.size * CurrentZoomScale;

        // ContentがViewportより小さい場合は中央固定（動かす必要がない）
        float maxX = Mathf.Max(0f, (contentSize.x - viewportSize.x) * 0.5f);
        float maxY = Mathf.Max(0f, (contentSize.y - viewportSize.y) * 0.5f);

        Vector2 pos = _content.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
        pos.y = Mathf.Clamp(pos.y, -maxY, maxY);
        _content.anchoredPosition = pos;
    }
}
