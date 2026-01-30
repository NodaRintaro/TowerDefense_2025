using System;
using TowerDefenseDeckData;
using UniRx;

namespace OutGame.Home
{
    public interface IHomeView
    {
        public event Action OnClickHomeCharacterChange;
        public event Action<ScreenType> OnClickPanelButton;
        public event Action OnClickHomeButton;
        public event Action OnClickBackButton;
        public event Action OnSyncTeamView;
        public event Action OnSyncCharacterSelectView;
        public event Action OnClickBattle;

        void Initialize(GenericCharacterData genericCharacterData);
        void SetHomeCharacterImage(UnityEngine.Sprite sprite);
        void ChangePanelView(ScreenType screenType);
        void RecallPanelView();
        void InitializeTeamView(GenericCharacterData _);
        void TeamBuildView(GenericCharacterData genericCharacterData);
        void CharacterSelectView(GenericCharacterData genericCharacterData);
        void SaveDeck(GenericCharacterData genericCharacterData);
        void GachaPanelView();
        void TrainingPanelView(AddressableStageDataRepository addressableStageDataRepository);
        void StoryPanelView();
        void SettingsPanelView();
        
        void SetLoading(bool isLoading);
        void ShowError(string message);
        void ClearError();
    }
}