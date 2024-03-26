using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;


namespace Yeon
{
    [CustomEditor(typeof(BlessData))]
    public class BlessDataInspector : Editor
    {
        private BlessData _blessData;
        private TabComponent tabComponent;

        private void OnEnable()
        {
            _blessData = (BlessData)target;
            if (_blessData.LevelAttributes != null && _blessData.LevelAttributes.TFieldNames != null)
            {
                var tabMessages = new TabMessage[_blessData.LevelAttributes.TFieldNames.Length];
                for (int i = 0; i < _blessData.LevelAttributes.TFieldNames.Length; i++)
                {
                    string fieldName = _blessData.LevelAttributes.TFieldNames[i];
                    tabMessages[i] = new TabMessage(fieldName,
                        () => { _blessData.LevelAttributes.CreateFieldArray(i); },
                        () => { _blessData.LevelAttributes.RemoveFieldArray(i); });
                }
                tabComponent = new TabComponent(tabMessages);
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (tabComponent != null)
            {
                tabComponent.Draw();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_blessData);
            }
        }

        private void OnDisable()
        {
            /*
            if (tabComponent != null)
            {
                tabComponent.Destroy();
            }
            */
        }
    }
}
