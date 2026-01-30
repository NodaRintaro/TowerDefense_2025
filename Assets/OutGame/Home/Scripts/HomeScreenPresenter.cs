using System;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace OutGame.Home
{
    public class HomeScreenPresenter : IStartable, IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly IHomeView _view;
        private readonly HomeScreenModel _model;
        private readonly ScreenNavigator _navigator;
        
        
        [Inject]
        public HomeScreenPresenter(IHomeView view, HomeScreenModel model)
        {
            Debug.Log("HomeScreenPresenter Constructor");
            _view = view;
            _model = model;
            _navigator = new ScreenNavigator(_view);
        }
        public void Start()
        {
            //初期化
            _model.OnDataLoaded.Subscribe(_view.Initialize).AddTo(_disposables);
            _model.OnDataLoaded.Subscribe(_view.CharacterSelectView).AddTo(_disposables);
            _model.OnDataLoaded.Subscribe(_view.InitializeTeamView).AddTo(_disposables);
            //ホーム画面キャラ切り替え
            _view.OnClickHomeCharacterChange += _model.ChangeHomeCharacter;
            _model.OnChangedHomeCharacter.Subscribe(_view.SetHomeCharacterImage).AddTo(_disposables);
            //スクリーン切り替え
            _view.OnClickHomeButton += _navigator.GoHome;
            _view.OnClickBackButton += _navigator.Back;
            _view.OnClickPanelButton += _navigator.Navigate;
            //チーム編成画面
            _view.OnSyncTeamView += () =>
            {
                _view.TeamBuildView(_model.TeamFormation());
            };
            //キャラ選択画面
            _view.OnSyncCharacterSelectView += () =>
            {
                _view.SaveDeck(_model.TeamFormation());
            };
            _view.OnClickBattle += _view.StoryPanelView;
        }

        public void Dispose()
        {
            _view.OnClickHomeCharacterChange -= _model.ChangeHomeCharacter;
            _view.OnClickHomeButton -= _navigator.GoHome;
            _view.OnClickBackButton -= _navigator.Back;
            _view.OnClickPanelButton -= _navigator.Navigate;
            _disposables.Dispose();
        }

            
    }
}