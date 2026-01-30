using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using TowerDefenseDeckData;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OutGame.Home
{
    public class HomeScreenView : MonoBehaviour, IHomeView, IDisposable
    {
#if UNITY_EDITOR
        [SerializeField] private GameObject _bgm;
        [SerializeField] private GameObject _se;
#endif
        [Header("ホーム画面")] [SerializeField] private Button _characterChangeButton;
        [SerializeField] private Image _homeCharacterImage;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _backButton;
        public event Action OnClickHomeCharacterChange;
        public event Action OnClickHomeButton;
        public event Action OnClickBackButton;

        [Header("画面遷移ボタン")] [SerializeField] private List<ScreenButtonEntry> _screenButtons;
        [SerializeField] private GameObject _defaultPanel;
        [SerializeField] private Button _trainingButton;
        private Dictionary<ScreenType, ScreenButtonEntry> _screenButtonEntries = new();
        public event Action<ScreenType> OnClickPanelButton;
        private GameObject _currentScreen;

        [Header("チーム編成画面")] [SerializeField] private Transform _selectButtonParent;
        [SerializeField] private Transform _selectDeckParent;
        [SerializeField] private List<TeamFormationEntry> _teamFormationEntries;
        [SerializeField] private Sprite _emptySprite;
        private List<TeamFormationCell> _teamFormationCells = new List<TeamFormationCell>();
        private List<Image> _selectDeckImageList = new List<Image>();
        private List<Image> _selectImageList = new List<Image>();
        private List<uint> _selectedCharacterIds = new List<uint>();
        private ReactiveProperty<int> _currentSelectTeamIndex = new ReactiveProperty<int>(0);
        private int _maxDeckIndex = 5;
        public ReactiveProperty<int> CurrentSelectSelectDecIndex => _currentSelectTeamIndex;
        public List<uint> SelectedCharacterIds => _selectedCharacterIds;
        public event Action<int> OnClickChangeTeam;
        public event Action OnClickPlusTeamIndex;
        public event Action OnClickMinusTeamIndex;
        public event Action OnSyncTeamView;

        [Header("キャラ選択画面")] [SerializeField] private GameObject _characterListParent;
        [SerializeField] private GameObject _characterSelectListParent;
        [SerializeField] private TeamFormationCell _characterView;
        [SerializeField] private Button _selectDecideButton;

        [Header("キャラ詳細表示用UI")]
        [SerializeField] private List<CharacterListView> _characterListViewList;
        private List<(uint, TeamFormationCell)> _characterListView = new List<(uint, TeamFormationCell)>();
        public List<(uint, TeamFormationCell)> CharacterListView => _characterListView;
        public event Action OnSyncCharacterSelectView;
        private CharacterBaseData _noneBaseData = new CharacterBaseData();
        private TowerDefenseCharacterData _noneData = new TowerDefenseCharacterData();

        [Header("戦闘UI")] [SerializeField] private Button _stageDecideButton;
        [SerializeField] private Button _battleStartButton;
        public event Action OnClickBattle;
        
        #region UnityFunctions

        private void Awake()
        {
#if UNITY_EDITOR
            _bgm.SetActive(false);
            _se.SetActive(false);
#endif
            //画面遷移関連
            _homeButton.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);
            _currentScreen = _defaultPanel;
            _characterChangeButton.onClick.AddListener(() => OnClickHomeCharacterChange?.Invoke());
            _homeButton.onClick.AddListener(() => OnClickHomeButton?.Invoke());
            _backButton.onClick.AddListener(() => OnClickBackButton?.Invoke());
            _trainingButton.onClick.AddListener(()=> SceneChanger.SceneChange("RaisingSimulation"));
            Debug.Log("HomeScreenView Awake");
            foreach (var screenButton in _screenButtons)
            {
                _screenButtonEntries.Add(screenButton.ScreenType, screenButton);
                screenButton.Button.onClick.AddListener(() => OnClickPanelButton?.Invoke(screenButton.ScreenType));
                if (screenButton.ScreenType == ScreenType.TeamFormation)
                    screenButton.Button.onClick.AddListener(() => OnSyncTeamView?.Invoke());
                screenButton.Panel.SetActive(false);
            }
            _stageDecideButton.onClick.AddListener(() => OnClickBattle?.Invoke());
            _stageDecideButton.onClick.AddListener(()=>OnClickPanelButton?.Invoke(ScreenType.TeamFormation));
            OnClickPanelButton += ChangePanelView;
            _defaultPanel.SetActive(true);

            //チーム編成関連
            foreach (Transform selectButton in _selectButtonParent.transform)
            {
                _selectImageList.Add(selectButton.GetComponent<Image>());
            }

            foreach (Transform item in _selectDeckParent.transform)
            {
                _selectDeckImageList.Add(item.GetComponent<Image>());
            }

            foreach (var teamFormationButton in _teamFormationEntries)
            {
                switch (teamFormationButton.Type)
                {
                    case ChangeType.Renew:
                        teamFormationButton.Button.onClick.AddListener(() =>
                            OnClickChangeTeam?.Invoke(teamFormationButton.Index));
                        break;
                    case ChangeType.Plus:
                        teamFormationButton.Button.onClick.AddListener(() => OnClickPlusTeamIndex?.Invoke());
                        break;
                    case ChangeType.Minus:
                        teamFormationButton.Button.onClick.AddListener(() => OnClickMinusTeamIndex?.Invoke());
                        break;
                }

                teamFormationButton.Button.onClick.AddListener(() =>
                    OnSyncTeamView?.Invoke());
            }
            _noneBaseData.InitData(999, "None", 1, 1, 1, 1, 1, "", 1, 1, new());
            _noneData.SetBaseData(_noneBaseData);
            _selectDecideButton.onClick.AddListener(()=> OnSyncCharacterSelectView?.Invoke());
            _selectDecideButton.onClick.AddListener(() => OnClickBackButton?.Invoke());
            _selectDecideButton.onClick.AddListener(() => OnSyncTeamView?.Invoke());
            _battleStartButton.onClick.AddListener(()=> SceneChanger.SceneChange("InGame"));
            OnClickChangeTeam += ChangeDeck;
            OnClickPlusTeamIndex += PlusDeckIndex;
            OnClickMinusTeamIndex += MinusDeckIndex;
        }

        public void Initialize(GenericCharacterData genericCharacterData)
        {
            OnSyncTeamView?.Invoke();
        }

        private void OnDisable()
        {
            OnClickPanelButton -= ChangePanelView;
            OnClickChangeTeam -= ChangeDeck;
            OnClickPlusTeamIndex -= PlusDeckIndex;
            OnClickMinusTeamIndex -= MinusDeckIndex;
        }

        #endregion

        #region HOME画面

        //Home画面のキャラクター画像を設定
        public void SetHomeCharacterImage(Sprite sprite)
        {
            _homeCharacterImage.sprite = sprite;

            _homeCharacterImage.rectTransform.pivot = new Vector2(
                _homeCharacterImage.sprite.pivot.x / _homeCharacterImage.sprite.rect.width,
                _homeCharacterImage.sprite.pivot.y / _homeCharacterImage.sprite.rect.height);
        }

        //スクリーンを変更する
        public void ChangePanelView(ScreenType screenType)
        {
            if (_currentScreen != null) _currentScreen.SetActive(false);

            if (screenType != ScreenType.Home)
            {
                _homeButton.gameObject.SetActive(true);
                _backButton.gameObject.SetActive(true);
            }
            else
            {
                _homeButton.gameObject.SetActive(false);
                _backButton.gameObject.SetActive(false);
                _battleStartButton.gameObject.SetActive(false);
            }


            var next = _screenButtonEntries[screenType].Panel;
            if (next == null)
            {
                Debug.LogError($"Panel not found: {screenType}");
                next = _defaultPanel;
            }

            next.SetActive(true);
            _currentScreen = next;
        }

        #endregion

        public void RecallPanelView()
        {
        }

        #region チーム編成画面

        public void TeamBuildView(GenericCharacterData genericCharacterData)
        {
            //編成キャラクターの表示
            _selectedCharacterIds.Clear();
            foreach (var character in genericCharacterData.JsonCharacterDeckDataRepository.RepositoryData.GetData(_currentSelectTeamIndex.Value)
                         .trainedCharacterDeck)
            {
                if (genericCharacterData.TrainedCharacterDataBase.TowerDefenseCharacterDataDict.Any(x =>
                        x.Key == character.CharacterID))
                    _selectedCharacterIds.Add(character.CharacterID);
            }

            Debug.Log("_selectedCharacterIds.Count:" + _selectedCharacterIds.Count);
            Debug.Log("_currentSelectDecIndex:" + _currentSelectTeamIndex);
            for (int i = 0; i < _selectedCharacterIds.Count; i++)
            {
                _selectImageList[i].sprite =
                    genericCharacterData.CharacterImageDataRegistry.GetCharacterSprite(_selectedCharacterIds[i],
                        CharacterSpriteType.MiniCard);
            }

            for (int i = _selectedCharacterIds.Count; i < CharacterDeckData.DeckLength; i++)
            {
                _selectImageList[i].sprite = _emptySprite;
            }

            DeckDataLoader.SetDeck(genericCharacterData.JsonCharacterDeckDataRepository.RepositoryData.GetData(_currentSelectTeamIndex.Value));
        }

        //表示するデッキを変更
        public void ChangeDeck(int num)
        {
            _currentSelectTeamIndex.Value = num;
            SelectDeckView();
        }

        //現状の右のデッキに変更
        public void PlusDeckIndex()
        {
            _currentSelectTeamIndex.Value = (_currentSelectTeamIndex.Value + 1) % _maxDeckIndex;
            SelectDeckView();
        }


        //現状の左のデッキに変更
        public void MinusDeckIndex()
        {
            _currentSelectTeamIndex.Value = (_currentSelectTeamIndex.Value - 1 + _maxDeckIndex) % _maxDeckIndex;
            SelectDeckView();
        }

        //UIの表示変更
        private void SelectDeckView()
        {
            for (int i = 0; i < _maxDeckIndex; i++)
            {
                if (i == _currentSelectTeamIndex.Value)
                {
                    _selectDeckImageList[i].DOFade(1f, 0f);
                }
                else
                {
                    _selectDeckImageList[i].DOFade(0f, 0f);
                }
            }
        }


        #region キャラ選択画面

        public void CharacterSelectView(GenericCharacterData genericCharacterData)
        {
            foreach (var character in genericCharacterData.TrainedCharacterDataBase.TowerDefenseCharacterDataDict)
            {
                TeamFormationCell obj = Instantiate(_characterView, _characterSelectListParent.transform);
                obj.IconImage.sprite = genericCharacterData.CharacterImageDataRegistry.GetCharacterSprite(
                    character.Key, CharacterSpriteType.MiniCard);
                TeamFormationCell obj2 = Instantiate(_characterView, _characterListParent.transform);
                obj2.IconImage.sprite = genericCharacterData.CharacterImageDataRegistry.GetCharacterSprite(
                    character.Key, CharacterSpriteType.MiniCard);
                obj.GetComponent<CharacterViewButton>().OnClick += () =>
                {
                    ViewDetail(obj.CharacterId, genericCharacterData);
                };
                obj2.GetComponent<CharacterViewButton>().OnClick += () =>
                {
                    ViewDetail(obj.CharacterId, genericCharacterData);
                };
                obj.SetCharacterId(character.Key);
                _characterListView.Add((character.Key, obj));
            }

            foreach (var item in _characterListView)
            {
                foreach (var chara in _characterListView.FindAll(x => x.Item1 == item.Item1))
                {
                    chara.Item2.GetComponent<CharacterViewButton>().OnClick += () =>
                    {
                        EditDeck(item.Item1);
                    };
                }
            }

            ViewDetail(1, genericCharacterData);
        }

        private void ViewDetail(uint characterId, GenericCharacterData genericCharacterData)
        {
            TowerDefenseCharacterData characterData = new();
            for (int i = 19; i >= 0; i--)
            {
                genericCharacterData.TrainedCharacterDataBase.TryGetCharacterDict(
                    out characterData, characterId, (uint)i);
                if (characterData != null)
                {
                    break;
                }
            }

            for (int i = 0; i < _characterListViewList.Count; i++)
            {
                //キャラクター画像
                _characterListViewList[i].CharacterViewImage.sprite = genericCharacterData.CharacterImageDataRegistry.GetCharacterSprite(
                    characterId, CharacterSpriteType.OverAllView);
                _characterListViewList[i].CharacterViewImage.rectTransform.pivot = new Vector2(
                    _characterListViewList[i].CharacterViewImage.sprite.pivot.x / _characterListViewList[i].CharacterViewImage.sprite.rect.width,
                    _characterListViewList[i].CharacterViewImage.sprite.pivot.y / _characterListViewList[i].CharacterViewImage.sprite.rect.height);

                //キャラクター名
                _characterListViewList[i].CharacterNameText.text = characterData.CharacterName;

                //ジョブ
                _characterListViewList[i].JobIconImage.sprite =
                    genericCharacterData.CharacterJobImageDataRegistry.GetData(characterData.CharacterRole);

                //コスト
                _characterListViewList[i].CostText.text = characterData.Cost.ToString();

                //ブロック数


                //パワー
                RankType powerRank =
                    RankCalculator.GetCurrentRank(characterData.TotalPower,
                        CharacterParameterRankRateData.RankRateDict);
                uint maxPower = CharacterParameterRankRateData.RankRateDict[powerRank + 1];
                _characterListViewList[i].PowerText.text = characterData.TotalPower.ToString();
                _characterListViewList[i].MaxPowerText.text = "/" + maxPower;
                _characterListViewList[i].PowerSlider.DOValue((float)characterData.TotalPower / maxPower, 1f).SetEase(Ease.OutSine);
                _characterListViewList[i].PowerRankImage.sprite = genericCharacterData.RankImageDataRegistry.GetData(powerRank);

                //知力
                RankType intelligenceRank = RankCalculator.GetCurrentRank(characterData.TotalIntelligence,
                    CharacterParameterRankRateData.RankRateDict);
                uint maxIntelligence = CharacterParameterRankRateData.RankRateDict[intelligenceRank + 1];
                _characterListViewList[i].IntelligenceText.text = characterData.TotalIntelligence.ToString();
                _characterListViewList[i].MaxIntelligenceText.text = "/" + maxIntelligence;
                _characterListViewList[i].IntelligenceSlider.DOValue((float)characterData.TotalIntelligence / maxIntelligence, 1f)
                    .SetEase(Ease.OutSine);
                _characterListViewList[i].IntelligenceRankImage.sprite = genericCharacterData.RankImageDataRegistry.GetData(powerRank);

                //体力
                RankType physicalRank = RankCalculator.GetCurrentRank(characterData.TotalPhysical,
                    CharacterParameterRankRateData.RankRateDict);
                uint maxPhysical = CharacterParameterRankRateData.RankRateDict[physicalRank + 1];
                _characterListViewList[i].PhysicalText.text = characterData.TotalPhysical.ToString();
                _characterListViewList[i].MaxPhysicalText.text = "/" + maxPhysical;
                _characterListViewList[i].PhysicalSlider.DOValue((float)characterData.TotalPhysical / maxPhysical, 1f).SetEase(Ease.OutSine);
                _characterListViewList[i].PhysicalRankImage.sprite = genericCharacterData.RankImageDataRegistry.GetData(powerRank);
                //素早さ
                RankType speedRank =
                    RankCalculator.GetCurrentRank(characterData.TotalSpeed,
                        CharacterParameterRankRateData.RankRateDict);
                uint maxSpeed = CharacterParameterRankRateData.RankRateDict[speedRank + 1];
                _characterListViewList[i].SpeedText.text = characterData.TotalSpeed.ToString();
                _characterListViewList[i].MaxSpeedText.text = "/" + maxSpeed;
                _characterListViewList[i].SpeedSlider.DOValue((float)characterData.TotalSpeed / maxSpeed, 1f).SetEase(Ease.OutSine);
                _characterListViewList[i].SpeedRankImage.sprite = genericCharacterData.RankImageDataRegistry.GetData(powerRank);
            }
        }
        
        //デッキデータを上書き
        public async void SaveDeck(GenericCharacterData genericCharacterData)
        {
            for (int i = 0;
                 i < _selectedCharacterIds.Count; i++)
            {
                genericCharacterData.TrainedCharacterDataBase.TryGetCharacterDict(
                    out TowerDefenseCharacterData currentCharacterData,
                    _selectedCharacterIds[i], 0);
                

                genericCharacterData.JsonCharacterDeckDataRepository.RepositoryData
                    .CharacterDeckHolder[_currentSelectTeamIndex.Value].SetData(i, currentCharacterData);
            }

            for (int i = _selectedCharacterIds.Count;
                 i < CharacterDeckData.DeckLength; i++)
            {
                genericCharacterData.JsonCharacterDeckDataRepository.RepositoryData
                    .CharacterDeckHolder[_currentSelectTeamIndex.Value].SetData(i, _noneData);
            
            }

            await genericCharacterData.JsonCharacterDeckDataRepository.DataSaveAsync();
        }

        // デッキデータにキャラクターをセットする
        private void EditDeck(uint characterId)
        {
            if (CheckDeck(characterId))
            {
                RemoveDeck(characterId);
            }
            else
            {
                AddDeck(characterId);
            }

            SelectCharacterView(characterId);
        }

        private void AddDeck(uint characterId)
        {
            if (_selectedCharacterIds.Contains(characterId)) return;
            _selectedCharacterIds.Add(characterId);
        }

        private void RemoveDeck(uint characterId)
        {
            _selectedCharacterIds.Remove(characterId);
        }

        // デッキにキャラクターが存在するか確認する
        private bool CheckDeck(uint characterId)
        {
            return _selectedCharacterIds.Contains(characterId);
        }

        //選んだキャラの表示を変える
        private void SelectCharacterView(uint characterId)
        {
            if (_selectedCharacterIds.Contains(characterId))
            {
                foreach (var item in _characterListView.FindAll(x => x.Item1 == characterId))
                {
                    item.Item2.SelectOverlay.enabled = true;
                }
            }
            else
            {
                foreach (var item in _characterListView.FindAll(x => x.Item1 == characterId))
                {
                    item.Item2.SelectOverlay.enabled = false;
                }
            }
        }

        public void InitializeTeamView(GenericCharacterData _)
        {
            foreach (var obj in _characterListView)
            {
                obj.Item2.SelectOverlay.enabled = false;
                if (_selectedCharacterIds.Contains(obj.Item1))
                    obj.Item2.SelectOverlay.enabled = true;
            }
        }

        #endregion

        #endregion

        public void GachaPanelView()
        {
        }

        public void TrainingPanelView(AddressableStageDataRepository addressableStageDataRepository)
        {
        }

        public void StoryPanelView()
        {
            _battleStartButton.gameObject.SetActive(true);
        }

        public void SettingsPanelView()
        {
        }

        public void SetLoading(bool isLoading)
        {
        }

        public void ShowError(string message)
        {
        }

        public void ClearError()
        {
        }

        public void Dispose()
        {
            _characterChangeButton.onClick.RemoveAllListeners();
        }

        [Serializable]
        public class ScreenButtonEntry
        {
            public ScreenType ScreenType;
            public Button Button;
            public GameObject Panel;
        }

        [Serializable]
        public class TeamFormationEntry
        {
            public ChangeType Type;
            public Button Button;
            public int Index;
        }

        public enum ChangeType
        {
            Plus,
            Minus,
            Renew
        }
    }

    public enum ScreenType
    {
        None,
        Home,
        Recall,
        TeamFormation,
        CharacterSelect,
        CharacterCollection,
        Training,
        Gacha,
        Story,
        Settings
    }

    [Serializable]
    public class CharacterListView
    {
        [Header("キャラ詳細表示用UI")] [SerializeField]
        public Image CharacterViewImage;

        [SerializeField] public TMP_Text CharacterNameText;
        [Header("パラメーター")] [SerializeField] public TMP_Text CostText;
        [SerializeField] public TMP_Text BlockCountText;
        [SerializeField] public Image JobIconImage;
        [Header("パワー")] [SerializeField] public TMP_Text PowerText;
        [SerializeField] public TMP_Text MaxPowerText;
        [SerializeField] public Slider PowerSlider;
        [SerializeField] public Image PowerRankImage;
        [Header("知力")] [SerializeField] public TMP_Text IntelligenceText;
        [SerializeField] public TMP_Text MaxIntelligenceText;
        [SerializeField] public Slider IntelligenceSlider;
        [SerializeField] public Image IntelligenceRankImage;
        [Header("体力")] [SerializeField] public TMP_Text PhysicalText;
        [SerializeField] public TMP_Text MaxPhysicalText;
        [SerializeField] public Slider PhysicalSlider;
        [SerializeField] public Image PhysicalRankImage;
        [Header("素早さ")] [SerializeField] public TMP_Text SpeedText;
        [SerializeField] public TMP_Text MaxSpeedText;
        [SerializeField] public Slider SpeedSlider;
        [SerializeField] public Image SpeedRankImage;
    }
}