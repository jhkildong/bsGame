using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEditorInternal;

/// <summary>
/// json���Ͽ� ������ ���� ����ϴ� Dictionary <string, List<LevetUpData>>
/// </summary>
public static class BlessLevelTableDict
{
    public static Dictionary<string, List<LevelUpData>> Dict = new Dictionary<string, List<LevelUpData>>();
}


[CustomEditor(typeof(BlessData))]
public class Bless2DataEditor : Editor
{
    ReorderableList reorderableList;    //����Ʈ�� editor�� �׷����°��� �����ϱ� ���� ReoderableList�� ���
    BlessData myScript;

    ///<summary>
    /// ����Ƽ �⺻�� lvDataList�� +������ �߰��� ����Ʈ�� ������ ��Ұ� ����Ǿ� ������
    /// �̸� ���� �ϱ� ���� �����ϴ� �ʱⰪ(����Ʈ �߰��� ���� 0���� �ʱ�ȭ�� ��Ұ� �߰���)
    ///</summary>
    void OnEnable()
    {
        var prop = serializedObject.FindProperty("_lvDataList");

        reorderableList = new ReorderableList(serializedObject, prop);
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight*9;
        
        //����Ʈ�� ��Ҹ� �׸��� �ݹ��Լ� ����
        reorderableList.drawElementCallback =
          (rect, index, isActive, isFocused) => {
              var element = prop.GetArrayElementAtIndex(index);
              rect.height -= 4;     
              rect.y += 2;
              EditorGUI.PropertyField(rect, element);
          };
        //add��ư�� ������ �� ����Ǵ� �ݹ��Լ� ����
        reorderableList.onAddCallback = (list) =>
        {
            prop.arraySize++;
            list.index = prop.arraySize - 1;
            var element = prop.GetArrayElementAtIndex(list.index);
            element.FindPropertyRelative("name").stringValue = "";
            element.FindPropertyRelative("defaultValue").floatValue = 0;
            element.FindPropertyRelative("levelUpType").enumValueIndex = 0;
            var levelUpTable = element.FindPropertyRelative("levelUpTable");
            levelUpTable.arraySize = 7;
            for (int i = 0; i < LevelUpData.MAX_LEVEL; i++)
            {
                levelUpTable.GetArrayElementAtIndex(i).floatValue = 0;
            }
        };
        //����Ʈ�� ����� �׸��� �ݹ��Լ� ����
        reorderableList.drawHeaderCallback = (rect) =>
          EditorGUI.LabelField(rect, prop.displayName);
        myScript = (BlessData)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();  // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.

        SerializedProperty iterator = serializedObject.GetIterator();
        
        //��ũ��Ʈ �ʵ带 ���
        iterator.NextVisible(true);
        using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
        {
            EditorGUILayout.PropertyField(iterator, true);
        }

        while (iterator.NextVisible(false))  // _lvDataList�� ������ ������ ������Ƽ�� ���
        {
            if (iterator.name != "_lvDataList")
            {
                EditorGUILayout.PropertyField(iterator, true);
            }
        }
        //reorderableList�� ������ ����Ʈ�� �׸�
        reorderableList.DoLayoutList();

        #region Save, Load ��ư
        EditorGUILayout.BeginHorizontal();
        //Save ��ư�� ������ BlessLevelTable.json ���Ͽ� ����
        if (GUILayout.Button("Save"))
        {
            string json = File.ReadAllText(FilePath.BlessLevelTableJson);
            //json ������ �о BlessLevelTableDict.Dict�� ����
            BlessLevelTableDict.Dict = JsonConvert.DeserializeObject<Dictionary<string, List<LevelUpData>>>(json);
            
            //json ���Ͽ� ����
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,  //null���� json�� ����
                Formatting = Formatting.Indented                //�鿩����
            };
            
            if(BlessLevelTableDict.Dict.ContainsKey(myScript.name))
            {   
                //����� ���� ������ ������Ʈ
                BlessLevelTableDict.Dict[myScript.name] = myScript.LvDataList;
            }
            else
            {
                //������ �߰�
                BlessLevelTableDict.Dict.Add(myScript.name, myScript.LvDataList);
            }
            json = JsonConvert.SerializeObject(BlessLevelTableDict.Dict, settings);
            File.WriteAllText(FilePath.BlessLevelTableJson, json);

            //Key.cs�� �̸� �߰�
            if (!File.Exists(FilePath.KeyCs))   //������ ������ ����
            {
                Debug.Log("������ ã�� �� �����ϴ�: " + FilePath.KeyCs);
                return;
            }
            string scriptContent = File.ReadAllText(FilePath.KeyCs);
            //����Ʈ�� �ִ� �̸����� Key.cs�� �߰�
            foreach (LevelUpData data in myScript.LvDataList)
            {
                if(data.name == "" || data.name == null)    //�̸��� ������ ��ŵ
                {
                    continue;
                }
                if (scriptContent.Contains(data.name))      //�̹� ��ϵ� �̸��̸� ��ŵ
                {
                    continue;
                }
                string key = $"\tpublic const string {data.name} = \"{data.name}\";\n"; //\tpublic const string dataName = "dataName";\n
                
                int lastLineIndex = scriptContent.LastIndexOf('\n');
                scriptContent = scriptContent.Insert(lastLineIndex, key); //������ �ٳѱ� ���� �߰�

                File.WriteAllText(FilePath.KeyCs, scriptContent); //��ũ��Ʈ ���� ������Ʈ
                AssetDatabase.Refresh(); //���� ����(���ΰ�ħ)
            }
        }

        if (GUILayout.Button("Load"))
        {
            if (File.Exists(FilePath.BlessLevelTableJson))
            {
                string json = File.ReadAllText(FilePath.BlessLevelTableJson);
                //json ������ �о BlessLevelTableDict.Dict�� ����
                BlessLevelTableDict.Dict = JsonConvert.DeserializeObject<Dictionary<string, List<LevelUpData>>>(json);

                //�̸��� ���� �����Ͱ� ������ LvDataList�� ����
                if (BlessLevelTableDict.Dict.ContainsKey(myScript.name))
                {
                    myScript.LvDataList = BlessLevelTableDict.Dict[myScript.name];
                }
                else
                {
                    Debug.Log("����� �����Ͱ� �����ϴ�.");
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Open BlessLevelTable.json"))
        {
            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(FilePath.BlessLevelTableJson);
            if (asset == null)
            {
                Debug.LogError("File not found at " + FilePath.BlessLevelTableJson);
                return;
            }

            AssetDatabase.OpenAsset(asset);
        }
        if(GUILayout.Button("Open Key.cs"))
        {
            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(FilePath.KeyCs);
            if (asset == null)
            {
                Debug.LogError("File not found at " + FilePath.BlessLevelTableJson);
                return;
            }

            AssetDatabase.OpenAsset(asset);
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}