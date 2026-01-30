using System;
using System.Collections.Generic;
using UnityEngine;

namespace OutGame.Home
{
    public sealed class ScreenNavigator
    {
        private readonly IHomeView _view;
        private readonly Stack<ScreenType> _history = new();

        private ScreenType _current = ScreenType.Home;
        public ScreenType Current => _current;

        public event Action<ScreenType> OnChanged;

        public ScreenNavigator(IHomeView view, ScreenType start = ScreenType.Home)
        {
            _view = view;
            _current = start;
            _history.Clear();
            //_view.ChangePanelView(_current);
            OnChanged?.Invoke(_current);
        }

        public void Navigate(ScreenType next)
        {
            if (next == _current) return;

            if(next == ScreenType.CharacterSelect)
                _history.Push(_current);

            _current = next;
            _view.ChangePanelView(_current);
            OnChanged?.Invoke(_current);
        }

        public void Back()
        {
            if (_history.Count == 0)
            {
                // 履歴がないならホームへ（好みで何もしないでもOK）
                GoHome();
                return;
            }

            var prev = _history.Pop();
            _current = prev;
            _view.ChangePanelView(_current);
            OnChanged?.Invoke(_current);
        }

        public void GoHome()
        {
            _history.Clear();
            _current = ScreenType.Home;
            _view.ChangePanelView(_current);
            OnChanged?.Invoke(_current);
        }
    }
}