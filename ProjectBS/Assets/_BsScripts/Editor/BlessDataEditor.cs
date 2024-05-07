using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEditorInternal;

/// <summary>
/// json파일에 저장을 위해 사용하는 Dictionary <string, List<LevetUpData>>
/// </summary>
public static class BlessLevelTableDict
{
    public static Dictionary<string, List<LevelUpData>> Dict = new Dictionary<string, List<LevelUpData>>();
    
}

[CustomEditor(typeof(BlessData))]
public class Bless2DataEditor : Editor
{
    ReorderableList reorderableList;    //리스트가 editor에 그려지는것을 제어하기 위해 ReoderableList를 사용
    BlessData myScript;

    ///<summary>
    /// 유니티 기본은 lvDataList를 +눌러서 추가시 리스트의 마지막 요소가 복사되어 생성됨
    /// 이를 방지 하기 위해 설정하는 초기값(리스트 추가시 전부 0으로 초기화된 요소가 추가됨)
    ///</summary>
    void OnEnable()
    {
        var prop = serializedObject.FindProperty("_lvDataList");

        reorderableList = new ReorderableList(serializedObject, prop);
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight*9;
        
        //리스트의 요소를 그리는 콜백함수 설정
        reorderableList.drawElementCallback =
          (rect, index, isActive, isFocused) => {
              var element = prop.GetArrayElementAtIndex(index);
              rect.height -= 4;     
              rect.y += 2;
              EditorGUI.PropertyField(rect, element);
          };
        //add버튼을 눌렀을 떄 실행되는 콜백함수 설정
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
        //리스트의 헤더를 그리는 콜백함수 설정
        reorderableList.drawHeaderCallback = (rect) =>
          EditorGUI.LabelField(rect, prop.displayName);
        myScript = (BlessData)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();  // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.

        SerializedProperty iterator = serializedObject.GetIterator();
        
        //스크립트 필드를 출력
        iterator.NextVisible(true);
        using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
        {
            EditorGUILayout.PropertyField(iterator, true);
        }

        while (iterator.NextVisible(false))  // _lvDataList를 제외한 나머지 프로퍼티를 출력
        {
            if (iterator.name != "_lvDataList")
            {
                EditorGUILayout.PropertyField(iterator, true);
            }
        }
        //reorderableList로 설정된 리스트를 그림
        reorderableList.DoLayoutList();

        #region Save, Load 버튼
        EditorGUILayout.BeginHorizontal();
        //Save 버튼을 누르면 BlessLevelTable.json 파일에 저장
        if (GUILayout.Button("Save"))
        {
            string json = File.ReadAllText(FilePath.BlessLevelTableJson);
            //json 파일을 읽어서 BlessLevelTableDict.Dict에 저장
            BlessLevelTableDict.Dict = JsonConvert.DeserializeObject<Dictionary<string, List<LevelUpData>>>(json);

            //json 파일에 저장

            var settings = new JsonSerializerSettings   //세팅값
            {
                NullValueHandling = NullValueHandling.Include,  //null값도 json에 포함
                Formatting = Formatting.Indented                //들여쓰기
            };
            
            if(BlessLevelTableDict.Dict.ContainsKey(myScript.name))
            {   
                //저장된 값이 있으면 업데이트
                BlessLevelTableDict.Dict[myScript.name] = myScript.LvDataList;
            }
            else
            {
                //없으면 추가
                BlessLevelTableDict.Dict.Add(myScript.name, myScript.LvDataList);
            }
            json = JsonConvert.SerializeObject(BlessLevelTableDict.Dict, settings);
            File.WriteAllText(FilePath.BlessLevelTableJson, json);

            //Key.cs에 이름 추가
            if (!File.Exists(FilePath.KeyCs))   //파일이 없으면 리턴
            {
                Debug.Log("파일을 찾을 수 없습니다: " + FilePath.KeyCs);
                return;
            }
            string scriptContent = File.ReadAllText(FilePath.KeyCs);
            //리스트에 있는 이름들을 Key.cs에 추가
            foreach (LevelUpData data in myScript.LvDataList)
            {
                if(data.name == "" || data.name == null)    //이름이 없으면 스킵
                {
                    continue;
                }
                if (scriptContent.Contains(data.name))      //이미 등록된 이름이면 스킵
                {
                    continue;
                }
                string key = $"\tpublic const string {data.name} = \"{data.name}\";\n"; //\tpublic const string dataName = "dataName";\n
                
                int lastLineIndex = scriptContent.LastIndexOf('\n');
                scriptContent = scriptContent.Insert(lastLineIndex, key); //마지막 줄넘김 전에 추가

                File.WriteAllText(FilePath.KeyCs, scriptContent); //스크립트 파일 업데이트
                AssetDatabase.Refresh(); //에셋 갱신(새로고침)
            }
        }

        if (GUILayout.Button("Load"))
        {
            if (File.Exists(FilePath.BlessLevelTableJson))
            {
                string json = File.ReadAllText(FilePath.BlessLevelTableJson);
                //json 파일을 읽어서 BlessLevelTableDict.Dict에 저장
                BlessLevelTableDict.Dict = JsonConvert.DeserializeObject<Dictionary<string, List<LevelUpData>>>(json);

                //이름이 같은 데이터가 있으면 LvDataList에 저장
                if (BlessLevelTableDict.Dict.ContainsKey(myScript.name))
                {
                    myScript.LvDataList = BlessLevelTableDict.Dict[myScript.name];
                }
                else
                {
                    Debug.Log("저장된 데이터가 없습니다.");
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        #region SaveAll, LoadAll 버튼
        EditorGUILayout.BeginHorizontal();
        //Save 버튼을 누르면 BlessLevelTable.json 파일에 저장
        if (GUILayout.Button("SaveAll"))
        {
            string json = File.ReadAllText(FilePath.BlessLevelTableJson);
            //json 파일을 읽어서 BlessLevelTableDict.Dict에 저장
            BlessLevelTableDict.Dict = JsonConvert.DeserializeObject<Dictionary<string, List<LevelUpData>>>(json);

            LevelUpDescription.GetLevelUpDescriptionToJson();

            //json 파일에 저장
            var settings = new JsonSerializerSettings   //세팅값
            {
                NullValueHandling = NullValueHandling.Include,  //null값도 json에 포함
                Formatting = Formatting.Indented                //들여쓰기
            };

            BlessData[] blessDatas = Resources.LoadAll<BlessData>(FilePath.Bless);    //모든 BlessData를 로드

            string[] defaultDescription = new string[7] { "", "", "", "", "", "", "" };
            foreach(BlessData blessData in blessDatas)
            {
                if (BlessLevelTableDict.Dict.ContainsKey(blessData.name))
                {
                    //저장된 값이 있으면 업데이트
                    BlessLevelTableDict.Dict[blessData.name] = blessData.LvDataList;
                }
                else
                {
                    //없으면 추가
                    BlessLevelTableDict.Dict.Add(blessData.name, blessData.LvDataList);
                }
                if(!LevelUpDescription.DescriptionDic.ContainsKey(blessData.ID))
                    LevelUpDescription.DescriptionDic.Add(blessData.ID, defaultDescription);
            }

            json = JsonConvert.SerializeObject(BlessLevelTableDict.Dict, settings);
            File.WriteAllText(FilePath.BlessLevelTableJson, json);

            string DescriptionJson = JsonConvert.SerializeObject(LevelUpDescription.DescriptionDic, settings);
            File.WriteAllText("Assets/_BsData/Resources/Json/BlessLevelUpDescription.json", DescriptionJson);

            //Key.cs에 이름 추가
            if (!File.Exists(FilePath.KeyCs))   //파일이 없으면 리턴
            {
                Debug.Log("파일을 찾을 수 없습니다: " + FilePath.KeyCs);
                return;
            }
            string scriptContent = File.ReadAllText(FilePath.KeyCs);
            
            foreach(BlessData blessData in blessDatas)
            {
                //리스트에 있는 이름들을 Key.cs에 추가
                foreach (LevelUpData data in blessData.LvDataList)
                {
                    if (data.name == "" || data.name == null)    //이름이 없으면 스킵
                    {
                        continue;
                    }
                    if (scriptContent.Contains(data.name))      //이미 등록된 이름이면 스킵
                    {
                        continue;
                    }
                    string key = $"\tpublic const string {data.name} = \"{data.name}\";\n"; //\tpublic const string dataName = "dataName";\n

                    int lastLineIndex = scriptContent.LastIndexOf('\n');
                    scriptContent = scriptContent.Insert(lastLineIndex, key); //마지막 줄넘김 전에 추가

                    File.WriteAllText(FilePath.KeyCs, scriptContent); //스크립트 파일 업데이트
                    AssetDatabase.Refresh(); //에셋 갱신(새로고침)
                }
            }
        }

        if (GUILayout.Button("LoadAll"))
        {
            if (File.Exists(FilePath.BlessLevelTableJson))
            {
                string json = File.ReadAllText(FilePath.BlessLevelTableJson);
                //json 파일을 읽어서 BlessLevelTableDict.Dict에 저장
                BlessLevelTableDict.Dict = JsonConvert.DeserializeObject<Dictionary<string, List<LevelUpData>>>(json);

                BlessData[] blessDatas = Resources.LoadAll<BlessData>(FilePath.Bless);    //모든 BlessData를 로드

                foreach(BlessData blessData in blessDatas)
                {
                    //이름이 같은 데이터가 있으면 LvDataList에 저장
                    if (BlessLevelTableDict.Dict.ContainsKey(blessData.name))
                    {
                        blessData.LvDataList = BlessLevelTableDict.Dict[blessData.name];
                    }
                    else
                    {
                        Debug.Log("저장된 데이터가 없습니다.");
                    }
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Open BlessLevelTable.json"))
        {
            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(FilePath.BlessLevelTableJson);
            if (asset == null)
            {
                Debug.LogError("File not found at " + FilePath.BlessLevelTableJson);
                return;
            }

            AssetDatabase.OpenAsset(asset);
        }
        if (GUILayout.Button("Open Key.cs"))
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