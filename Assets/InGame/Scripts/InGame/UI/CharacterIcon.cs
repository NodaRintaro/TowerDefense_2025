using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour, IPointerDownHandler
{
    [field: System.NonSerialized] private int id;
    [SerializeField] private Text _costText;
    [SerializeField] private Image _icon;
    public void SetID(int ID)
    {
        id = ID;
        _costText.text = InGameManager.Instance.UnitDataManager.GetCharacterData(id).Cost.ToString();
        _icon.color = InGameManager.Instance.UnitDataManager.GetCharacterData(id).Color;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        InGameManager.Instance.SelectCharacter(id);
    }
}
