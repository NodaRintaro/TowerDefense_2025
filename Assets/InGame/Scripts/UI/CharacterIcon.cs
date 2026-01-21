using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour, IPointerDownHandler
{
    private int id;
    [SerializeField] private Text _costText;
    [SerializeField] private Image _icon;
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
}
