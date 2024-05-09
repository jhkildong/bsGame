using UnityEngine;
using Newtonsoft.Json;
using System.IO;

using System.Collections.Generic;

public static class LevelUpDescription
{
    public static Dictionary<int, string[]> DescriptionDic = new();

    public static void GetLevelUpDescriptionToJson()
    {
        var jsonTextFile = Resources.Load<TextAsset>(FilePath.BlessLevelUpDiscriptionJson);

        //json ������ �о DescriptionDic�� ����
        DescriptionDic = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(jsonTextFile.ToString());
    }
}
