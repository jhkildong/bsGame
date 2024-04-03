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

            //BlessData�� LevelProp�� BlessŬ������ LevelProp�� �����ϵ��� ����(Editor������ ���)
            _blessData.LevelProp = _bless.LevelProp;
            
            //_bless�� LevelProp�� null�ΰ��
            if(_bless.LevelProp == null)
            {
                _bless.LevelProp.Initialize1(_bless);
            }
            //LevelProp�� null�� �ƴѰ��(�ʵ� ��(Reflection)�ؼ� �ٸ��� �ʱ�ȭ)
            else
            {
                _bless.LevelProp.Initialize2(_bless);
            }
            //LevelProp�� �ְ�, Public Property�� ���� ��� TabComponent ����
            if(_bless.LevelProp != null && _bless.LevelProp.PropertyNames != null)
            {
                TabMessage[] tabMessages = new TabMessage[_bless.LevelProp.PropertyNames.Length];
                for (int i = 0; i < _bless.LevelProp.PropertyNames.Length; i++)
                {
                    int index = i;
                    string propertyName = _bless.LevelProp.PropertyNames[i];
                    tabMessages[i] = new TabMessage(propertyName,
                                               () => { _bless.LevelProp.CreatePropertyArray(index); },
                                               () => { _bless.LevelProp.RemovePropertyArray(index); });
                }
                tabComponent = new TabComponent(tabMessages, _bless.LevelProp.States);
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
            serializedObject.Dispose();
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
