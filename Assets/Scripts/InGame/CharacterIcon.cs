using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour, IPointerDownHandler
{
    [field: System.NonSerialized] private int id;
    [SerializeField] private Text _costText;
    public void SetID(int ID)
    {
        id = ID;
        _costText.text = InGameManager.Instance.CharacterDataManager.GetCharacterData(id).Cost.ToString();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        InGameManager.Instance.SelectCharacter(id);
    }
}
