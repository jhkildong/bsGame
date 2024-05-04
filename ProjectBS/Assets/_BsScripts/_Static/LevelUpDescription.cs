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
        //json ������ �о DescriptionDic�� ����
        DescriptionDic = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(json);
    }
}
