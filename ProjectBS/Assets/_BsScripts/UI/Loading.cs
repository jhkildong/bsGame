using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public static int targetScene;
    public Slider myLoadingBar;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingScene(targetScene));
    }

    public static void LoadScene(int scene)
    {
        targetScene = scene;
        SceneManager.LoadScene(1);
    }

    IEnumerator LoadingScene(int scene)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(targetScene);
        ao.allowSceneActivation = false;
        myLoadingBar.value = 0.0f;

        while (myLoadingBar.value < 1.0f)
        {
            yield return StartCoroutine(UpdatingSlider(ao.progress / 0.9f));
        }
        //yield return new WaitForSeconds(1.0f);
        ao.allowSceneActivation = true;
    }

    IEnumerator UpdatingSlider(float v)
    {
        while (myLoadingBar.value < v)
        {
            myLoadingBar.value += Time.deltaTime;
            yield return null;
        }
        myLoadingBar.value = v;
    }

}

