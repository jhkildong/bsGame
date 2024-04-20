using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Enum에서 지정한 범위내의 값만 인스펙터에 표시
/// </summary>
public class EnumRangeAttribute : PropertyAttribute
{
    public int Min { get; private set; }
    public int Max { get; private set; }

    public EnumRangeAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }
}

[CustomPropertyDrawer(typeof(EnumRangeAttribute))]
public class EnumRangeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumRangeAttribute range = (EnumRangeAttribute)attribute;

        if (property.propertyType == SerializedPropertyType.Enum)
        {
            string[] enumNames = property.enumNames;
            List<string> enumDescriptions = new List<string>();
            List<int> enumIndices = new List<int>();
            Type enumType = fieldInfo.FieldType;

            for (int i = 0; i < enumNames.Length; i++)
            {
                int enumValue = (int)Enum.Parse(enumType, enumNames[i]);
                if (enumValue >= range.Min && enumValue <= range.Max)
                {
                    enumDescriptions.Add(GetDescription((Enum)Enum.Parse(enumType, enumNames[i])));
                    enumIndices.Add(i);
                }
            }

            int selectedIndex = enumIndices.IndexOf(property.enumValueIndex);
            int newSelectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, enumDescriptions.ToArray());
            if (newSelectedIndex != selectedIndex)
            {
                property.enumValueIndex = enumIndices[newSelectedIndex];
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use EnumRange with enum.");
        }
    }

    private string GetDescription(Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0)
        {
            return attributes[0].Description;
        }
        else
        {
            return value.ToString();
        }
    }
}
