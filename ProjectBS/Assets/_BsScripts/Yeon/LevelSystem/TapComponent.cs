using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Yeon
{
    public class TabComponent
    {
        private List<TabContainer> _tabs = new List<TabContainer>();

        public TabComponent(TabMessage[] drawActions)
        {
            foreach (var m in drawActions)
                _tabs.Add(new TabContainer(m));
        }

        public void Draw()
        {
            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.HorizontalScope())
                    foreach (var t in _tabs)
                        t.DrawToggle();

                GUILayout.Space(10);
                foreach (var t in _tabs)
                    t.SetAction();
            }
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
            bool isActivated = false;
  
            OnTabActivated = () =>
            {
                if(!isActivated)
                {
                    isActivated = true;
                    onTabActivated?.Invoke();
                    EditorPrefs.SetBool(Title, true);
                }
            };
            OnTabDeactivated = () =>
            {
                if(isActivated)
                {
                    isActivated = false;
                    onTabDeactivated?.Invoke();
                    EditorPrefs.SetBool(Title, false);
                }
            };
        }
    }

    public class TabContainer
    {
        private TabMessage _message;
        private bool _state;

        public TabContainer(TabMessage message)
        {
            _message = message;
            _state = EditorPrefs.GetBool(_message.Title, false);
        }

        public bool DrawToggle()
        {
            bool newState = GUILayout.Toggle(_state, _message.Title, "Button", GUILayout.Height(50));
            if (newState != _state)
            {
                _state = newState;
                EditorPrefs.SetBool(_message.Title, _state);
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
    }
}
