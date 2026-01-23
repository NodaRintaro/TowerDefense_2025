using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Slider _hpBarObject; //HPバー
    private float _maxHp;       //HPバーの最大値

    public void Init(float maxHp)
    {
        _maxHp = maxHp;
        _hpBarObject.value = 1.0f;
    }

    public void UpdateHp(float hp)
    {
        _hpBarObject.value = hp / _maxHp;
    }
}
