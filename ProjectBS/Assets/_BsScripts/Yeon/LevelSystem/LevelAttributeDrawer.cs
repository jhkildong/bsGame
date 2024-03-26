using UnityEngine;
using UnityEditor;

namespace Yeon
{
    [CustomEditor(typeof(LevelAttribute<>), true)]
    public class LevelAttributeEditor<T> : Editor
    {
        LevelAttribute<T> myTarget;
        bool[] toggleStates;

        private void OnEnable()
        {
            myTarget = (LevelAttribute<T>)target;
            toggleStates = new bool[myTarget.TFieldNames.Length];
            for (int i = 0; i < myTarget.TFieldNames.Length; i++)
            {
                toggleStates[i] = EditorPrefs.GetBool(myTarget.TFieldNames[i], false);
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            for (int i = 0; i < myTarget.TFieldNames.Length; i++)
            {
                bool newState = EditorGUILayout.Toggle(myTarget.TFieldNames[i], toggleStates[i]);
                if (newState != toggleStates[i])
                {
                    toggleStates[i] = newState;
                    EditorPrefs.SetBool(myTarget.TFieldNames[i], newState);
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}