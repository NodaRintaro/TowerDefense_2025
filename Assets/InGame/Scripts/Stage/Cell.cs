using UnityEngine;
/// <summary>
/// ゲーム内でユニットを配置するセル
/// </summary>
public class Cell : MonoBehaviour
{
    private Material _material;
    private bool _isCharacter = false;
    public bool IsCharacter => _isCharacter;
    private void Start()
    {
        _material = transform.GetChild(0).GetComponent<Renderer>().material;
    }

    public void OnPointerEnter()
    {
        if(IsCharacter)
            _material.color = Color.red;
        else
            _material.color = Color.green;
    }

    public void OnPointerExit()
    {
        _material.color = Color.white;
    }
    /// <summary>
    /// セルにユニットを配置できるか
    /// </summary>
    /// <returns></returns>
    public bool CanPlaceCharacter()
    {
        return !IsCharacter;
    }

    public void SetCharacter(GameObject character)
    {
        character.transform.parent = transform;
        character.transform.localPosition = Vector3.zero;
        _isCharacter = true;
        
        UnitBase unit = character.GetComponent<UnitBase>();
        // unit.Init();
        unit.OnRemovedEvent += OnUnitDead;
    }
    //配置されたユニットが倒されたときに呼ばれる。
    void OnUnitDead()
    {
        _isCharacter = false;
    }
}
