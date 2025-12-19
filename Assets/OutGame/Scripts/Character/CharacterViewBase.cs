using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterViewBase : MonoBehaviour
{
    [SerializeField] protected Image _characterImage;

    public void SetImage(Sprite charaSprite)
    {
        _characterImage.sprite = charaSprite;
    }
}
