using UnityEngine;
/// <summary>
/// ゲーム内でユニットを配置するセル
/// </summary>
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
        
        UnitBase unit = character.GetComponent<UnitBase>();
        unit.Init();
        unit.OnDeathEvent += OnUnitDead;
        return true;
    }
    //配置されたユニットが倒されたときに呼ばれる。
    void OnUnitDead()
    {
        _isCharacter = false;
    }
}
