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

        public TabComponent(params TabMessage[] drawActions)
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
                        t.DrawButton();

                GUILayout.Space(10);
                foreach (var t in _tabs)
                    t.DrawContent();
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
        bool isActivated = false;

        public TabMessage(string title, Action onTabActivated, Action onTabDeactivated)
        {
            Title = title;
            isActivated = EditorPrefs.GetBool(Title, false);

            OnTabActivated = () =>
            {
                onTabActivated?.Invoke();
                EditorPrefs.SetBool(Title, true);
            };
            OnTabDeactivated = () =>
            {
                onTabDeactivated?.Invoke();
                EditorPrefs.SetBool(Title, false);             
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
        }

        public bool DrawButton(Action disableRest = null)
        {
            bool previousState = _state;
            _state = GUILayout.Toggle(_state, _message.Title, "Button", GUILayout.Height(50));
            if (_state != previousState && _state)
            {
                disableRest?.Invoke();
            }

            return _state;
        }

        public void DrawContent()
        {
            if (_state)
                _message.OnTabActivated?.Invoke();
        }

        public void Destroy()
        {
            _message.OnTabActivated = null;
            _message = null;
            _message.OnTabDeactivated = null;
        }
    }
}
