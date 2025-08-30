using Unity.VisualScripting;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private Material _defaultMaterial;
    private bool _isCharacter = false;
    public bool IsCharacter => _isCharacter;
    private void Start()
    {
        _defaultMaterial = transform.GetChild(0).GetComponent<Renderer>().material;
    }

    public void OnPointerEnter()
    {
        if(IsCharacter)
            _defaultMaterial.color = Color.red;
        else
            _defaultMaterial.color = Color.green;
    }

    public void OnPointerExit()
    {
        _defaultMaterial.color = Color.white;
    }

    public bool SetCharacter(GameObject character)
    {
        if (IsCharacter) return false;
        character.transform.parent = transform;
        character.transform.localPosition = Vector3.zero;
        _isCharacter = true;
        character.GetComponent<UnitBase>().Init();
        return true;
    }
}
