using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Yeon
{
    public class TabComponent
    {
        [SerializeField] private List<TabContainer> _tabs = new List<TabContainer>();
        [SerializeField] private bool[] toggleStates;

        public TabComponent(TabMessage[] drawActions, bool[] states)
        {
            foreach (var m in drawActions)
                _tabs.Add(new TabContainer(m));
            toggleStates = states;
        }

        public void Draw()
        {
            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.HorizontalScope())
                    for (int i = 0; i < _tabs.Count; i++)
                    {
                        toggleStates[i] = _tabs[i].DrawToggle(toggleStates[i]);
                    }
                foreach (var t in _tabs)
                    t.SetAction();
            }
        }
        public void Destroy()
        {
            _tabs.ForEach(t => t.Destroy());
            _tabs.Clear();
        }
    }

    public class TabMessage
    {
        public Action OnTabActivated;
        public Action OnTabDeactivated;
        public string Title;

        public TabMessage(string title, Action onTabActivated, Action onTabDeactivated)
        {
            Title = title;

            OnTabActivated = () =>
            {
                onTabActivated?.Invoke();
            };
            OnTabDeactivated = () =>
            {
                onTabDeactivated?.Invoke();
            };
        }
    }

    public class TabContainer
    {
        private TabMessage _message;
        private bool _state = false;

        public TabContainer(TabMessage message)
        {
            _message = message;
        }

        public bool DrawToggle(bool state)
        {
            _state = state;
            bool newState = GUILayout.Toggle(_state, _message.Title, GUI.skin.button, GUILayout.Height(30));
            if (newState != _state)
            {
                _state = newState;
                SetAction();
            }
            return _state;
        }

        public void SetAction()
        {
            if (_state)
            {
                _message.OnTabActivated?.Invoke();
            }
            else
            {
                _message.OnTabDeactivated?.Invoke();
            }
        }
        public void Destroy()
        {
            _message.OnTabActivated = null;
            _message.OnTabDeactivated = null;
            _message = null;
        }
    }
}
