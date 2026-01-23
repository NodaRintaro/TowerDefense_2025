using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour, IPointerDownHandler
{
    private int id;
    [SerializeField] private Text _costText;
    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _reconfigureSlider;

    private Slider _slider;
    private float _duration;
    private float _timer;
    

    public void Init(int ID)
    {
        id = ID;
        PlayerUnitData unitData = InGameManager.Instance.UnitDeck.UnitDatas[id];
        _costText.text = unitData.Cost.ToString();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        InGameManager.Instance.SelectCharacter(id);
    }
    

    private void Awake()
    {
        _slider = _reconfigureSlider.GetComponent<Slider>();
        
        _reconfigureSlider.SetActive(false);
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime * InGameManager.Instance.TimeSpeed;
            _slider.value = _timer / _duration;
            
            if(_timer <= 0)
                _reconfigureSlider.SetActive(false);
        }
    }

    public void UpdateSlider(float duration)
    {
        _reconfigureSlider.SetActive(true);
        _duration = duration;
        _timer = _duration;
    }
}
