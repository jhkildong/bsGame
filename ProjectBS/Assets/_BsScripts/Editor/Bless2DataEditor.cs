using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEditorInternal;


public static class BlessLevelTableDict
{
    public static Dictionary<string, List<LevelUpData>> Dict = new Dictionary<string, List<LevelUpData>>();
}


[CustomEditor(typeof(Bless2Data))]
public class Bless2DataEditor : Editor
{
    ReorderableList reorderableList;

    void OnEnable()
    {
        var prop = serializedObject.FindProperty("_lvDataList");

        reorderableList = new ReorderableList(serializedObject, prop);
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight*9;
        reorderableList.drawElementCallback =
          (rect, index, isActive, isFocused) => {
              var element = prop.GetArrayElementAtIndex(index);
              rect.height -= 4;
              rect.y += 2;
              EditorGUI.PropertyField(rect, element);
          };
        reorderableList.onAddCallback = (list) =>
        {
            prop.arraySize++;
            list.index = prop.arraySize - 1;
            var element = prop.GetArrayElementAtIndex(list.index);
            element.FindPropertyRelative("name").stringValue = "";
            element.FindPropertyRelative("defaultValue").floatValue = 0;
            element.FindPropertyRelative("levelUpType").enumValueIndex = 0;
            for (int i = 0; i < LevelUpData.MAX_LEVEL; i++)
            {
                element.FindPropertyRelative("levelUpTable").GetArrayElementAtIndex(i).floatValue = 0;
            }
        };

        reorderableList.drawHeaderCallback = (rect) =>
          EditorGUI.LabelField(rect, prop.displayName);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true);  // Skip "m_Script" property
        while (iterator.NextVisible(false))  // Draw next visible property
        {
            if (iterator.name != "_lvDataList")
            {
                EditorGUILayout.PropertyField(iterator, true);
            }
        }
        reorderableList.DoLayoutList();


        Bless2Data myScript = (Bless2Data)target;

        if (GUILayout.Button("Save"))
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                Formatting = Formatting.Indented
            };

            if(BlessLevelTableDict.Dict.ContainsKey(myScript.name))
            {
                BlessLevelTableDict.Dict[myScript.name] = myScript.LvDataList;
            }
            else
            {
                BlessLevelTableDict.Dict.Add(myScript.name, myScript.LvDataList);
            }
            string json = JsonConvert.SerializeObject(BlessLevelTableDict.Dict, settings);
            File.WriteAllText(Application.dataPath + "/_BsData/Resources/BlessLevelTable.json", json);

            // ��ũ��Ʈ�� ��θ� �����մϴ�.
            string scriptPath = "Assets/_BsScripts/_Static/Key.cs";

            if (!File.Exists(scriptPath))
            {
                Debug.Log("������ ã�� �� �����ϴ�: " + scriptPath);
                return;
            }
            // ��ũ��Ʈ ������ ������ �н��ϴ�.
            string scriptContent = File.ReadAllText(scriptPath);
            foreach (LevelUpData data in myScript.LvDataList)
            {
                if(data.name == "" || data.name == null)
                {
                    continue;
                }
                // ��ũ��Ʈ�� �߰��� ������ �����մϴ�.
                string key = $"\tpublic const string {data.name} = \"{data.name}\";";
                if(scriptContent.Contains(data.name))
                {
                    continue;
                }
                // ������ �� ���� ������ �߰��մϴ�.
                int lastLineIndex = scriptContent.LastIndexOf('\n');
                if (lastLineIndex >= 0)
                {
                    scriptContent = scriptContent.Insert(lastLineIndex, key);
                }
                else
                {
                    scriptContent += key;
                }
                // ��ũ��Ʈ ������ ������Ʈ�մϴ�.
                File.WriteAllText(scriptPath, scriptContent);

                AssetDatabase.Refresh();
            }
        }

        if (GUILayout.Button("Load"))
        {
            string path = Application.dataPath + "/_BsData/Resources/BlessLevelTable.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                BlessLevelTableDict.Dict = JsonConvert.DeserializeObject<Dictionary<string, List<LevelUpData>>>(json);

                if (BlessLevelTableDict.Dict.ContainsKey(myScript.name))
                {
                    myScript.LvDataList = BlessLevelTableDict.Dict[myScript.name];
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}