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
        private Bless _bless;
        private TabComponent tabComponent;
        private TabComponent tabComponent1;

        private void OnEnable()
        {
            if ((_blessData = (BlessData)target) == null)
                return;
            if ((_bless = _blessData.Bless) == null)
                return;
            _blessData.LevelAttribute = _bless.LevelAttribute;
            _bless.LevelAttribute.Initilize(_bless, _blessData.ID);
            if(_bless.LevelAttribute != null && _bless.LevelAttribute.TPropertyNames != null && tabComponent == null)
            {
                TabMessage[] tabMessages = new TabMessage[_bless.LevelAttribute.TPropertyNames.Length];
                for (int i = 0; i < _bless.LevelAttribute.TPropertyNames.Length; i++)
                {
                    int index = i;
                    string propertyName = _bless.LevelAttribute.TPropertyNames[i];
                    tabMessages[i] = new TabMessage(propertyName,
                                               () => { _bless.LevelAttribute.CreatePropertyArray(_bless, index); },
                                               () => { _bless.LevelAttribute.RemovePropertyArray(index); });
                }
                tabComponent = new TabComponent(tabMessages);
            }
            {
                TabMessage[] tabMessages = new TabMessage[1];
                tabMessages[0] = new TabMessage("Level2", () => { _bless.LevelAttribute.LevelUp(2); }, () => { });
                tabComponent1 = new TabComponent(tabMessages);
            }
        }
            

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            tabComponent?.Draw();
            tabComponent1?.Draw();

            if (EditorGUI.EndChangeCheck())
            {
                // 변경 사항이 있으면 오브젝트를 'dirty'로 표시하여 변경 사항을 저장합니다.
                EditorUtility.SetDirty(_blessData);
                EditorUtility.SetDirty(_bless);
            }
        }
    }
}
