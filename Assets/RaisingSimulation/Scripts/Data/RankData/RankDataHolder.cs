using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RankData", menuName = "RankData/SupportCardDataHolder")]
public class RankDataHolder : ScriptableObject
{
    [SerializeField, Header("Rankのデータリスト")]
    RankData[] _rankList;

    public RankData[] RankList => _rankList;
}

public enum RankType
{
    None,
    F,
    E,
    D,
    C,
    B,
    A,
    S,
    SS,
    SSS
}
