using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Capture : MonoBehaviour
{
    public Camera Cam;
    public RenderTexture Rt;
    public Image Bg;

    void Start()
    {
        Cam = Camera.main;
    }

    public void Create()
    {
        StartCoroutine(CaputerImage());
    }


    IEnumerator CaputerImage()
    {
        yield return null;
        Texture2D texture = new Texture2D(Rt.width, Rt.height, TextureFormat.ARGB32, false, true);
        RenderTexture.active = Rt;
        texture.ReadPixels(new Rect(0, 0, Rt.width, Rt.height), 0, 0);
        
        yield return null;

        var data = texture.EncodeToPNG();
        string name = "Thumbnail";
        string extenstion = ".png";
        string path = Application.persistentDataPath + "/Thumbnail/";

        Debug.Log(path);

        if(!Directory.Exists(path)) Directory.CreateDirectory(path);

        File.WriteAllBytes(path + name + extenstion, data);

        yield return null;
    }

}
