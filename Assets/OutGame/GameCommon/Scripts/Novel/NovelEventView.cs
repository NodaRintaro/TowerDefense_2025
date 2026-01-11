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

    private List<Image> _subCharacterImageList = new();

    public DialogueText DialogueText => _dialogueText;

    //キャラクターの位置情報
    private readonly Vector3[] _characterImagePositionsData =
        new Vector3[]
        {
            new Vector3(-500, -150, 0),
            new Vector3(500, -150, 0),
            new Vector3(-650, -125, 0),
            new Vector3(650, -125, 0),
        };

    public void SetBackScreen(Sprite backScreen)
    {
        _backScreen.sprite = backScreen;
    }

    public void SetCharacterImage(Sprite mainCharacter, Sprite[] subCharacter = default)
    {
        int posCount = 0;
        _mainCharacterImage.sprite = mainCharacter;
        _mainCharacterImage.transform.position = _characterImagePositionsData[posCount];

        if(subCharacter != null)
        {
            posCount++;
            foreach( var character in subCharacter)
            {
                _subCharacters[posCount].sprite = character;
            }
        }
    }
}
