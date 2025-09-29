using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour, IPointerDownHandler
{
    private int id;
    [SerializeField] private Text _costText;
    [SerializeField] private Image _icon;
    public void SetID(int ID)
    {
        id = ID;
        UnitData unitData = InGameManager.Instance.CharacterDeck.GetCharacterData(id);
        _costText.text = unitData.cost.ToString();
        Color color = unitData.color;
        color.a = 1.0f;
        _icon.color = color;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        InGameManager.Instance.SelectCharacter(id);
    }
}
