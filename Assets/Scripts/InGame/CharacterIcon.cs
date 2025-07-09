using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour, IPointerDownHandler
{
    [field: System.NonSerialized] public int id;

    void Start()
    {
        InGameManager.Instance.onDropCharacter += OnRaycastTargetTrue;
        InGameManager.Instance.onSelectCharacter += OnRaycastTargetFalse;
    }
    void OnRaycastTargetFalse()
    {
        Debug.Log("CharacterIcon.OnRaycastTargetChange");
        GetComponent<Image>().raycastTarget = false;
    }

    void OnRaycastTargetTrue()
    {
        Debug.Log("CharacterIcon.OnRaycastTargetTrue");
        GetComponent<Image>().raycastTarget = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("CharacterButton.OnPointerDown");
        InGameManager.Instance.SelectCharacter(id);
    }
}
