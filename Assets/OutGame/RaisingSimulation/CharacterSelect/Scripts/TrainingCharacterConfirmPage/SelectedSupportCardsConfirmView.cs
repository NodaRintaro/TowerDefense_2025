using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SelectedSupportCardsConfirmView
{
    [SerializeField] Image[] _selectedSupportCardImages;

    [SerializeField] Image _cardSkillImage;

    [SerializeField] TMP_Text _cardSkillText;

    public Image[] SelectedSupportCardImages => _selectedSupportCardImages;
    public Image CardSkillImage => _cardSkillImage;
    public TMP_Text CardSkillText => _cardSkillText;
}
