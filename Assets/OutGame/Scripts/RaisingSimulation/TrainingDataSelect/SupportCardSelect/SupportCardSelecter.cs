using UnityEngine;
using VContainer;
using System;
using System.Linq;

[Serializable]
public class SupportCardSelecter
{
    [SerializeField, Header("選んだサポートカードのデータの保存先")]
    private SupportCardData[] _supportCardDeckData = new SupportCardData[_supportCardDatasNum];

    //現在選択中デッキの枠の番号
    private int _selectCardDeckNum = default;

    //サポートカードの編成枚数
    private const uint _supportCardDatasNum = 4;

    private SupportCardSelectController _controller;

    public int SelectCardDeckNum => _selectCardDeckNum;
    public SupportCardData[] SupportCardDeckData => _supportCardDeckData;

    [Inject]
    public SupportCardSelecter(SupportCardSelectController controller)
    {
        this._controller = controller;
    }

    /// <summary>
    /// デッキに入れたいカードを選択する処理
    /// </summary>
    /// <param name="cardID"></param>
    public void SelectSupportCard(SupportCardData supportCardData)
    {
        int selectCardDeckIndex = _selectCardDeckNum - 1;
        _supportCardDeckData[selectCardDeckIndex] = supportCardData;
    }

    /// <summary>
    /// カードを入れたいデッキの枠を選択
    /// </summary>
    public void SetSelectDeckNum(uint deckNum)
    {
        _selectCardDeckNum = (int)deckNum;
    }

    public bool IsSelectedCard(SupportCardData supportCardData)
    {
        if (_supportCardDeckData.Contains(supportCardData))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
