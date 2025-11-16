using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

[CreateAssetMenu(fileName = "RankData", menuName = "RankData/SupportCardDataHolder")]
public class RankDataHolder : ScriptableObject
{
    [Inject]
    RankDataHolder() { }

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
