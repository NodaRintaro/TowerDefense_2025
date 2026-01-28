using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;
using VContainer;

public class StaminaSlider : MonoBehaviour
{
    [Header("Sliderの設定")]
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Image _predictFillImage;
    [SerializeField] private TMP_Text _currentStaminaText;
    [SerializeField] private TMP_Text _maxStaminaText;

    [Header("アニメーションの設定")]
    [SerializeField] private float _normalAnimDuration = 0.3f;
    [SerializeField] float _predictAnimDuration = 0.15f;
    [SerializeField] private Ease _animEase = Ease.OutQuad;

    [Header("色の設定")]
    [SerializeField] private Color _increasePredictColor = new Color(0.5f, 1f, 0.5f, 0.5f);
    [SerializeField] Color _decreasePredictColor = new Color(1f, 0.5f, 0.5f, 0.5f);

    private uint _maxStamina = 100;
    private uint _currentStamina = 100;

    private Tween _currentTween;
    private Tween _predictTween;
    private float _targetStamina;
    
    private void OnEnable()
    {
        var lifeTimeScope = FindFirstObjectByType<RaisingSimulationDataContainer>();
        var trainingSaveDataRepository = lifeTimeScope.Container.Resolve<JsonTrainingSaveDataRepository>();
        
        Init();

        var saveData = trainingSaveDataRepository.RepositoryData;
        if (saveData != null)
        {
            SetMaxStamina(saveData.MaxStamina);
            SetStamina(saveData.CurrentStamina, true);
        }
    }

    /// <summary> 初期化 </summary>
    public void Init()
    {
        if (_staminaSlider != null)
        {
            _staminaSlider.maxValue = _maxStamina;
            _staminaSlider.value = _currentStamina;
            _targetStamina = _currentStamina;
        }

        if (_predictFillImage != null)
        {
            _predictFillImage.fillAmount = 1f;
            _predictFillImage.color = new Color(1f, 1f, 1f, 0f);
        }
    }


    /// <summary> スタミナを指定された数値分増減させる（正の数で増加、負の数で減少） </summary>
    public void ChangeStamina(int amount)
    {
        int newStamina = (int)_currentStamina + amount;
        newStamina = Mathf.Clamp(newStamina, 0, (int)_maxStamina);
        SetStamina((uint)newStamina);
    }

    /// <summary> スタミナを消費する </summary>
    public void ConsumeStamina(uint amount)
    {
        uint newStamina = _currentStamina > amount ? _currentStamina - amount : 0;
        SetStamina(newStamina);
    }

    /// <summary> スタミナを回復する </summary>
    public void RecoverStamina(uint amount)
    {
        uint newStamina = _currentStamina + amount;
        if (newStamina > _maxStamina)
        {
            newStamina = _maxStamina;
        }
        SetStamina(newStamina);
    }

    /// <summary> 最大スタミナを設定 </summary>
    public void SetMaxStamina(uint maxStamina)
    {
        _maxStamina = maxStamina;
        _maxStaminaText.text = "/" + maxStamina.ToString();

        if (_staminaSlider != null)
        {
            _staminaSlider.maxValue = maxStamina;
        }

        if (_currentStamina > _maxStamina)
        {
            _currentStamina = _maxStamina;
            SetStamina(_currentStamina);
        }
    }

    /// <summary> スタミナを設定する </summary>
    private void SetStamina(uint value, bool skipAnimation = false)
    {
        _targetStamina = Mathf.Clamp(value, 0, _maxStamina);
        _currentStaminaText.text = value.ToString();

        if(skipAnimation)
        {
            _currentTween?.Kill();
            _currentStamina = value;
            if (_staminaSlider != null)
            {
                _staminaSlider.value = _currentStamina;
            }
        }
        else
        {
            AnimateStaminaChange();
        }
    }

    /// <summary> スタミナ変更のアニメーション </summary>
    private void AnimateStaminaChange()
    {
        _currentTween?.Kill();

        _currentTween = DOTween.To(
            () => (float)_currentStamina,
            x => {
                _currentStamina = (uint)x;
                if (_staminaSlider != null)
                {
                    _staminaSlider.value = x;
                }
            },
            _targetStamina,
            _normalAnimDuration
        ).SetEase(_animEase);
    }

    /// <summary> 減少予測モーションを表示 </summary>
    public void ShowDecreasePrediction(uint amount)
    {
        if (_predictFillImage == null) return;

        _currentTween?.Complete();

        uint predictedValue = _currentStamina > amount ? _currentStamina - amount : 0;
        float predictedFillAmount = (float)predictedValue / _maxStamina;

        _predictFillImage.color = _decreasePredictColor;

        _predictTween?.Kill();
        _predictTween = DOTween.To(
            () => _predictFillImage.fillAmount,
            x => _predictFillImage.fillAmount = x,
            predictedFillAmount,
            _predictAnimDuration
        ).SetEase(Ease.OutCubic);
    }

    /// <summary> 増加予測モーションを表示 </summary>
    public void ShowIncreasePrediction(uint amount)
    {
        if (_predictFillImage == null) return;

        _currentTween?.Complete();

        uint predictedValue = _currentStamina + amount;
        if (predictedValue > _maxStamina)
        {
            predictedValue = _maxStamina;
        }
        float predictedFillAmount = (float)predictedValue / _maxStamina;

        _predictFillImage.color = _increasePredictColor;

        _predictTween?.Kill();
        _predictTween = DOTween.To(
            () => _predictFillImage.fillAmount,
            x => _predictFillImage.fillAmount = x,
            predictedFillAmount,
            _predictAnimDuration
        ).SetEase(Ease.OutCubic);
    }

    /// <summary> 予測モーションを非表示 </summary>
    public void HidePrediction()
    {
        if (_predictFillImage == null) return;

        _predictTween?.Kill();

        float currentFillAmount = (float)_currentStamina / _maxStamina;
        _predictTween = DOTween.To(
            () => _predictFillImage.fillAmount,
            x => _predictFillImage.fillAmount = x,
            currentFillAmount,
            _predictAnimDuration * 0.5f
        ).SetEase(Ease.OutQuad).OnComplete(() => {
            _predictFillImage.DOFade(0f, 0.1f);
        });
    }

    /// <summary> スタミナが最大かどうかを判定 </summary>
    public bool IsStaminaFull()
    {
        return _currentStamina >= _maxStamina;
    }

    /// <summary> スタミナが空かどうかを判定 </summary>
    public bool IsStaminaEmpty()
    {
        return _currentStamina == 0;
    }

    /// <summary> 破棄処理 </summary>
    public void Dispose()
    {
        _currentTween?.Kill();
        _predictTween?.Kill();
    }
}