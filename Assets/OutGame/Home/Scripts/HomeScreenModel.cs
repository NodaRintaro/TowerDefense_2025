using System;
using System.Collections.Generic;
using TowerDefenseDeckData;
using UniRx;
using UnityEngine;
using VContainer;

namespace OutGame.Home
{
    /// <summary>
    /// ホーム画面のデータとイベント通知を管理するモデルクラス
    /// </summary>
    public class HomeScreenModel : IDisposable
    {
        //データ群
        private JsonCharacterCollectionDataRepository _jsonCharacterCollectionDataRepository;
        private AddressableCharacterImageDataRepository _addressableCharacterImageDataRepository;
        private DataLoadCompleteNotifier _dataLoadCompleteNotifier;
        private List<uint> _characterIdList = new List<uint>();

        //ホーム画面用
        public Subject<Sprite> OnChangedHomeCharacter = new Subject<Sprite>();
        private int _currentHomeCharacterIndex = 0;
        private int _collectCharacterCount => _characterIdList.Count;

        //イメージデータ
        private CharacterImageDataRegistry _characterImageDataRegistry;
        private AddressableRankImageDataRepository _addressableRankImageDataRepository;
        private RankImageDataRegistry _rankImageDataRegistry;
        private AddressableCharacterJobImageDataRepository _addressableCharacterJobImageDataRepository;
        private CharacterJobImageDataRegistry _characterJobImageDataRegistry;

        //デッキデータ
        private JsonCharacterDeckDataRepository _jsonCharacterDeckDataRepository;
        private CharacterDeckDataBase _defaultDeckData;
        public Subject<GenericCharacterData> SyncTeamFormationData = new();

        //初期データ
        private AddressableCharacterDataRepository _addressableCharacterDataRepository;
        private CharacterBaseDataRegistry _characterBaseDataRegistry;

        //所持キャラデータ
        private JsonCharacterCollectionDataRepository _characterCollectionDataRepository;
        private CharacterCollectionData _characterCollectionData;

        //強化後キャラデータ
        private JsonTowerDefenseCharacterDataRepository _jsonTowerDefenseCharacterDataRepository;
        private TowerDefenseCharacterDataBase _trainedCharacterDataBase;


        public Subject<GenericCharacterData> OnDataLoaded = new Subject<GenericCharacterData>();

        [Inject]
        public HomeScreenModel(DataLoadCompleteNotifier dataLoadCompleteNotifier,
            JsonCharacterCollectionDataRepository jsonCharacterCollectionDataRepository,
            AddressableCharacterImageDataRepository addressableCharacterImageDataRepository,
            JsonCharacterDeckDataRepository jsonCharacterDeckDataRepository,
            JsonTowerDefenseCharacterDataRepository jsonTowerDefenseCharacterDataRepository,
            AddressableCharacterDataRepository addressableCharacterDataRepository,
            AddressableRankImageDataRepository rankImageDataRepository,
            AddressableCharacterJobImageDataRepository characterJobImageDataRepository)
        {
            _dataLoadCompleteNotifier = dataLoadCompleteNotifier;
            _jsonCharacterCollectionDataRepository = jsonCharacterCollectionDataRepository;
            _addressableCharacterImageDataRepository = addressableCharacterImageDataRepository;
            _jsonCharacterDeckDataRepository = jsonCharacterDeckDataRepository;
            _jsonTowerDefenseCharacterDataRepository = jsonTowerDefenseCharacterDataRepository;
            _addressableCharacterDataRepository = addressableCharacterDataRepository;
            _addressableRankImageDataRepository = rankImageDataRepository;
            _addressableCharacterJobImageDataRepository = characterJobImageDataRepository;
            _dataLoadCompleteNotifier.OnDataLoadComplete += DataLoad;
            DataLoad();
        }

        private void DataLoad()
        {
            if (_dataLoadCompleteNotifier.IsDataLoadComplete == false) return;
            _defaultDeckData = _jsonCharacterDeckDataRepository.RepositoryData;
            _characterBaseDataRegistry = _addressableCharacterDataRepository.RepositoryData;
            _characterCollectionData = _jsonCharacterCollectionDataRepository.RepositoryData;
            _trainedCharacterDataBase = _jsonTowerDefenseCharacterDataRepository.RepositoryData;
            _characterImageDataRegistry = _addressableCharacterImageDataRepository.RepositoryData;
            _rankImageDataRegistry = _addressableRankImageDataRepository.RepositoryData;
            _characterJobImageDataRegistry = _addressableCharacterJobImageDataRepository.RepositoryData;

            foreach (var characterId in _jsonCharacterCollectionDataRepository.RepositoryData.CollectionList)
            {
                _characterIdList.Add(characterId);
            }


            OnDataLoaded.OnNext(TeamFormation());
        }

        public void ChangeHomeCharacter()
        {
            _currentHomeCharacterIndex++;
            OnChangedHomeCharacter.OnNext(_addressableCharacterImageDataRepository.GetSprite(
                _characterIdList[_currentHomeCharacterIndex % _collectCharacterCount],
                CharacterSpriteType.OverAllView));
        }

        public GenericCharacterData TeamFormation()
        {
            return new GenericCharacterData(_characterImageDataRegistry, _jsonCharacterDeckDataRepository, _characterCollectionData,
                _trainedCharacterDataBase, _rankImageDataRegistry, _characterJobImageDataRegistry);
        }

        public void Dispose()
        {
            _dataLoadCompleteNotifier.OnDataLoadComplete -= DataLoad;
        }
    }

    public class GenericCharacterData
    {
        public CharacterImageDataRegistry CharacterImageDataRegistry;
        public JsonCharacterDeckDataRepository JsonCharacterDeckDataRepository;
        public CharacterCollectionData CharacterCollectionData;
        public TowerDefenseCharacterDataBase TrainedCharacterDataBase;
        public RankImageDataRegistry RankImageDataRegistry;
        public CharacterJobImageDataRegistry CharacterJobImageDataRegistry;

        public GenericCharacterData(CharacterImageDataRegistry characterImageDataRegistry,
            JsonCharacterDeckDataRepository jsonCharacterDeckDataRepository
            , CharacterCollectionData characterCollectionData, TowerDefenseCharacterDataBase trainedCharacterDataBase,
            RankImageDataRegistry rankImageDataRegistry, CharacterJobImageDataRegistry characterJobImageDataRegistry)
        {
            CharacterImageDataRegistry = characterImageDataRegistry;
            JsonCharacterDeckDataRepository = jsonCharacterDeckDataRepository;
            CharacterCollectionData = characterCollectionData;
            TrainedCharacterDataBase = trainedCharacterDataBase;
            RankImageDataRegistry = rankImageDataRegistry;
            CharacterJobImageDataRegistry = characterJobImageDataRegistry;
        }
    }
}