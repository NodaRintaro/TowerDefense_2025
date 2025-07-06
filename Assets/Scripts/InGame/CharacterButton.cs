using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    [field: System.NonSerialized] public int id;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    void OnClicked()
    {
        InGameManager.Instance.SelectCharacter(id);
    }
}
