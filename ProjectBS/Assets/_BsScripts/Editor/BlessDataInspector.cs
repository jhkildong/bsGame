using UnityEngine;
using UnityEditor;


namespace Yeon
{
    /// <summary>
    /// BlessData의 인스펙터를 커스터마이징
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

            //ScriptalbeObject에 바인딩 안되있으면 리턴
            if ((_blessData = (BlessData)target) == null || (_bless = _blessData.Bless) == null)
            {
                Debug.Log("BlessData or Bless is null");
                return;
            }

            //BlessData의 LevelProp가 Bless클래스의 LevelProp를 참조하도록 설정(Editor에서만 사용)
            _blessData.LevelProp = _bless.LevelProp;
            
            //_bless의 LevelProp가 null인경우
            if(_bless.LevelProp == null)
            {
                _bless.LevelProp.Initialize1(_bless);
            }
            //LevelProp가 null이 아닌경우(필드 비교(Reflection)해서 다르면 초기화)
            else
            {
                _bless.LevelProp.Initialize2(_bless);
            }
            //LevelProp가 있고, Public Property가 있을 경우 TabComponent 생성
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
