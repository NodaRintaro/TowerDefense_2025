using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    private int _id;
    private CharacterData _characterData;

    void Init()
    {
        _characterData = InGameManager.Instance.CharacterDataManager.GetCharacterData(_id);
    }
}