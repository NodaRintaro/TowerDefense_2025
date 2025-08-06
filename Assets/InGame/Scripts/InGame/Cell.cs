using UnityEngine;

public class Cell : MonoBehaviour
{
    Material defaultMaterial;
    private bool _isCharacter = false;
    public bool IsCharacter => _isCharacter;
    private void Start()
    {
        defaultMaterial = GetComponent<Renderer>().material;
    }

    public void OnPointerEnter()
    {
        if(IsCharacter)
            defaultMaterial.color = Color.red;
        else
            defaultMaterial.color = Color.green;
    }

    public void OnPointerExit()
    {
        defaultMaterial.color = Color.white;
    }

    public bool SetCharacter(GameObject character)
    {
        if (IsCharacter) return false;
        character.transform.parent = transform;
        character.transform.localPosition = Vector3.zero;
        _isCharacter = true;
        return true;
    }
}
