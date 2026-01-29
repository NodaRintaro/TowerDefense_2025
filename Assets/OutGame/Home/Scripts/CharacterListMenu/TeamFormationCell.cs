using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamFormationCell : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _hoverOverlay;
    [SerializeField] private Image _selectOverlay;
    [SerializeField] private Image _disableOverlay;
    private uint _characterId;
    private int _index;
    public uint CharacterId => _characterId;
    public int Index=> _index;
    public Image IconImage => _iconImage;
    public Image HoverOverlay => _hoverOverlay;
    public Image SelectOverlay => _selectOverlay;
    public Image DisableOverlay => _disableOverlay;
    
    public void SetCharacterId(uint id)
    {
        _characterId = id;
    }
    
    public void SetIndex(int index)
    {
        _index = index;
    }
}
