using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public static class LevelUpDescription
{
    public static Dictionary<int, string[]> DescriptionDic = new();

    public static void GetLevelUpDescriptionToJson()
    {
        string json = File.ReadAllText(FilePath.BlessLevelUpDiscriptionJson);
        //json 파일을 읽어서 DescriptionDic에 저장
        DescriptionDic = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(json);
    }
}
