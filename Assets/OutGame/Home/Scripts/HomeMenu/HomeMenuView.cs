using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HomeMenuView
{
    [Serializable]
    public struct ViewData
    {
        /// <summary> キャラクター育成へ進むボタン </summary>
        [Header("育成ゲームへすすむButton")]
        public Button CharacterTrainingButton;

        /// <summary> ストーリーへ進むボタン </summary>
        [Header("TDへすすむButton")]
        public Button InGameButton;

        /// <summary> キャラクター一覧へ進むボタン </summary>
        [Header("キャラクター一覧へすすむButton")]
        public Button CharacterDataTable;

        /// <summary> ストーリー回想へ進むボタン </summary>
        [Header("回想へすすむButton")]
        public Button StoryReminiscence;
    }
}
