using UnityEngine;
using UnityEditor;


namespace Yeon
{
    [CustomEditor(typeof(LevelAttribute<>), true)]
    public class LevelAttributeEditor<T> : Editor
    {
        private bool[] toggleStates;

        public override void OnInspectorGUI()
        {
            LevelAttribute<T> myTarget = (LevelAttribute<T>)target;

            if (toggleStates == null || toggleStates.Length != myTarget.TFieldNames.Length)
            {
                toggleStates = new bool[myTarget.TFieldNames.Length];
            }

            for (int i = 0; i < myTarget.TFieldNames.Length; i++)
            {
                toggleStates[i] = EditorGUILayout.Toggle(myTarget.TFieldNames[i], toggleStates[i]);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}

