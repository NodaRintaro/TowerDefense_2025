using UnityEngine;

public enum CharacterSpriteType
{
    [InspectorName("タイプが登録されていません")] None,
    [InspectorName("全体の立ち絵")] OverAllView,
    [InspectorName("カード")] Card,
    [InspectorName("カード(ミニ)")] MiniCard
}

