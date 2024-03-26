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
        private TabComponent tabComponent2;
        private TabComponent tabComponent3;

        private void OnEnable()
        {
            if ((_blessData = (BlessData)target) == null)
                return;
            if ((_bless = _blessData.Bless) == null)
                return;
            _blessData.LevelAttribute = _bless.LevelAttribute;
            _bless.LevelAttribute.Initilize(_bless, _blessData.ID);
            if (_bless.LevelAttribute != null && _bless.LevelAttribute.TFieldNames != null)
            {
                TabMessage[] tabMessages = new TabMessage[_bless.LevelAttribute.TFieldNames.Length];
                for (int i = 0; i < _bless.LevelAttribute.TFieldNames.Length; i++)
                {
                    int index = i;
                    string fieldName = _bless.LevelAttribute.TFieldNames[i];
                    tabMessages[i] = new TabMessage(fieldName,
                        () => { _bless.LevelAttribute.CreateFieldArray(index); },
                        () => { _bless.LevelAttribute.RemoveFieldArray(index); });
                }
                tabComponent = new TabComponent(tabMessages);
            }
            if(_bless.LevelAttribute != null && _bless.LevelAttribute.TPropertyNames != null)
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
                tabComponent2 = new TabComponent(tabMessages);
            }
        }
            

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            tabComponent?.Draw();
            tabComponent2?.Draw();
            
        }
    }
}
