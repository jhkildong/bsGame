using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LevelUpData))]
public class LevelUpDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (new EditorGUI.PropertyScope(position, label, property))
        {
            var nameProperty = property.FindPropertyRelative("name");
            var defaultValueProperty = property.FindPropertyRelative("defaultValue");
            var levelUpTypeProperty = property.FindPropertyRelative("levelUpType");
            var levelUpTableProperty = property.FindPropertyRelative("levelUpTable");

            var rowHeight = EditorGUIUtility.singleLineHeight;
            var rowGap = EditorGUIUtility.standardVerticalSpacing;

            var width = position.width * 0.1f;
            var spacing = position.width * 0.01f;

            var nameLabelRect = new Rect(position.x, position.y, width, rowHeight);
            var nameRect = new Rect(position.x + width, position.y, width * 2.5f, rowHeight);
            var defaultValueLabelRect = new Rect(position.x + width * 3.5f + spacing, position.y, width * 1.5f, rowHeight);
            var defaultValueRect = new Rect(position.x + width * 5f, position.y, width * 2f, rowHeight);
            var levelUpTypeLabelRect = new Rect(position.x + width * 7 + spacing, position.y, width, rowHeight);
            var levelUpTypeRect = new Rect(position.x + width * 8, position.y, width * 2, rowHeight);

            EditorGUI.LabelField(nameLabelRect, "이름");
            EditorGUI.PropertyField(nameRect, nameProperty, GUIContent.none);
            EditorGUI.LabelField(defaultValueLabelRect, "기본값");
            EditorGUI.PropertyField(defaultValueRect, defaultValueProperty, GUIContent.none);
            EditorGUI.LabelField(levelUpTypeLabelRect, "타입");
            EditorGUI.PropertyField(levelUpTypeRect, levelUpTypeProperty, GUIContent.none);

            levelUpTableProperty.arraySize = LevelUpData.MAX_LEVEL;
            for (int i = 0; i < LevelUpData.MAX_LEVEL; i++)
            {
                var levelUpTableElementRect = new Rect(position.x, position.y + (i + 1) * (rowHeight + rowGap), position.width, rowHeight);
                var levelUpTableElementProperty = levelUpTableProperty.GetArrayElementAtIndex(i);
                EditorGUI.PropertyField(levelUpTableElementRect, levelUpTableElementProperty, new GUIContent($"Level {i + 1}"));
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (LevelUpData.MAX_LEVEL + 1) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
    }
}