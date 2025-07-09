using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour, IPointerDownHandler
{
    [field: System.NonSerialized] public int id;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("CharacterButton.OnPointerDown");
        InGameManager.Instance.SelectCharacter(id);
    }
}
