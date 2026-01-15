using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NovelEventView : MonoBehaviour
{
    [Header("発言者の名前")]
    [SerializeField] private TMP_Text _speakerNameText;

    [Header("NovelText")]
    [SerializeField] private DialogueText _dialogueText;

    [Header("会話に参加しているキャラクターのイメージ")]
    [SerializeField] private Image _mainCharacterImage;

    [Header("会話に参加しているキャラクターのイメージ")]
    [SerializeField] private Image[] _subCharacters;

    [Header("背景")]
    [SerializeField] private Image _backScreen;

    [Header("キャラクターのポジションデータ")]
    [SerializeField] private RectTransform[] _characterImagePositionsData;

    private List<Image> _subCharacterImageList = new();

    public DialogueText DialogueText => _dialogueText;

    public void SetNameText(string name)
    {
        _speakerNameText.text = name;
    }

    public void SetBackScreen(Sprite backScreen)
    {
        _backScreen.sprite = backScreen;
    }

    public void SetCharacterImage(Sprite mainCharacter, Sprite[] subCharacter = null)
    {
        int posCount = 0;
        if(mainCharacter != null)
        {
            ShowCharacterImage(_mainCharacterImage);
            _mainCharacterImage.sprite = mainCharacter;
            _mainCharacterImage.transform.position = _characterImagePositionsData[posCount].position;
        }
        else
        {
            HideCharacterImage(_mainCharacterImage);
        }

        if (subCharacter != null)
        {
            posCount++;
            foreach (var character in subCharacter)
            {
                _subCharacters[posCount].sprite = character;
                _subCharacters[posCount].transform.position = _characterImagePositionsData[posCount].position;

                if (character == null)
                    HideCharacterImage(_subCharacters[posCount]);
                else
                    ShowCharacterImage(_subCharacters[posCount]);
            }
        }
        else
        {
            foreach (var character in _subCharacters)
            {
                HideCharacterImage(character);
            }
        }
    }

    private void ShowCharacterImage(Image image)
    {
        Color color = image.color;
        color.a = 255;
        image.color = color;
    }

    private void HideCharacterImage(Image image)
    {
        Color color = image.color;
        color.a = 0;
        image.color = color;
    }
}
