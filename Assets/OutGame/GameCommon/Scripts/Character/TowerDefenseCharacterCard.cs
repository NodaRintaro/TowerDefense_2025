using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TowerDefenseCharacterCard
{
    [SerializeField] Image _characterCard;
    [SerializeField] Image _rankImage;

    public void SetCardImage(Image cardImage)
    {
        _characterCard = cardImage;
    }

    public void SetRankImage(Image rankImage)
    {
        _rankImage = rankImage;
    }
}
