using UnityEngine;
using UnityEditor;


namespace Yeon
{
    /// <summary>
    /// BlessData�� �ν����͸� Ŀ���͸���¡
    /// </summary>
    [CustomEditor(typeof(BlessData))]
    public class BlessDataInspector : Editor
    {
        private BlessData _blessData;
        private Bless _bless;
        private TabComponent tabComponent;

        private void OnEnable()
        {
            _blessData = (BlessData)target;
            _bless = _blessData.Bless;

            //ScriptalbeObject�� ���ε� �ȵ������� ����
            if ((_blessData = (BlessData)target) == null || (_bless = _blessData.Bless) == null)
            {
                Debug.Log("BlessData or Bless is null");
                return;
            }

            //BlessData�� LevelAttribute�� BlessŬ������ LevelAttribute�� �����ϵ��� ����(Editor������ ���)
            _blessData.LevelAttribute = _bless.LevelAttribute;
            
            //_bless�� LevelAttribute�� null�ΰ��
            if(_bless.LevelAttribute == null)
            {
                _bless.LevelAttribute.Initialize1(_bless);
            }
            //LevelAttribute�� null�� �ƴѰ��(�ʵ� ��(Reflection)�ؼ� �ٸ��� �ʱ�ȭ)
            else
            {
                _bless.LevelAttribute.Initialize2(_bless);
            }
            //LevelAttribute�� �ְ�, Public Property�� ���� ��� TabComponent ����
            if(_bless.LevelAttribute != null && _bless.LevelAttribute.PropertyNames != null)
            {
                TabMessage[] tabMessages = new TabMessage[_bless.LevelAttribute.PropertyNames.Length];
                for (int i = 0; i < _bless.LevelAttribute.PropertyNames.Length; i++)
                {
                    int index = i;
                    string propertyName = _bless.LevelAttribute.PropertyNames[i];
                    tabMessages[i] = new TabMessage(propertyName,
                                               () => { _bless.LevelAttribute.CreatePropertyArray(index); },
                                               () => { _bless.LevelAttribute.RemovePropertyArray(index); });
                }
                tabComponent = new TabComponent(tabMessages, _bless.LevelAttribute.States);
            }
            
        }

        private void OnDisable()
        {
            if (_bless != null)
                EditorUtility.SetDirty(_bless);
            _bless = null;
            if (_blessData != null)
                EditorUtility.SetDirty(_blessData);
            _blessData = null;
            tabComponent?.Destroy();
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();
            tabComponent?.Draw();

            serializedObject.ApplyModifiedProperties();

            if(GUI.changed)
            {
                EditorUtility.SetDirty(_bless);
                EditorUtility.SetDirty(_blessData);
            }
        }
    }
}
