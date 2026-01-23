using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class DialogueText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textUI;
    [SerializeField] private float _charDelay = 0.05f;

    private CancellationTokenSource _currentCts;
    private bool _isSkipping = false;
    private Tweener _fadeTween;

    public void Init()
    {
        _textUI.text = " ";
    }

    public async UniTask ShowTextAsync(string message, CancellationToken ct = default)
    {
        CancelCurrentAnimation();

        _currentCts = CancellationTokenSource.CreateLinkedTokenSource(ct, this.GetCancellationTokenOnDestroy());
        _isSkipping = false;



        try
        {
            await TypewriterEffectAsync(message, _currentCts.Token);
        }
        catch (OperationCanceledException)
        {
            // キャンセルされた場合
        }
        finally
        {
            _currentCts?.Dispose();
            _currentCts = null;
        }
    }

    public void SkipText()
    {
        _isSkipping = true;
    }

    public void CancelCurrentAnimation()
    {
        _fadeTween?.Kill();
        _fadeTween = null;

        _currentCts?.Cancel();
        _currentCts?.Dispose();
        _currentCts = null;
    }

    private async UniTask TypewriterEffectAsync(string message, CancellationToken ct)
    {
        _textUI.text = "";
        _textUI.alpha = 0f;

        // DOTweenを手動で待機
        _fadeTween = _textUI.DOFade(1f, 0.3f);

        await UniTask.WaitWhile(() => _fadeTween != null && _fadeTween.IsActive() && !_fadeTween.IsComplete(),
            cancellationToken: ct);

        _fadeTween = null;

        foreach (char c in message)
        {
            ct.ThrowIfCancellationRequested();

            _textUI.text += c;

            if (_isSkipping)
            {
                _textUI.text = message;
                break;
            }

            await UniTask.Delay((int)(_charDelay * 1000), cancellationToken: ct);
        }
    }

    private void OnDestroy()
    {
        CancelCurrentAnimation();
    }
}