using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public Image imageComponent;
    public Image restartButton;
    public Image MainMenuButton;

    private float targetAlpha = 1f;
    

    void Start()
    {
       // Setup();
    }

    public void Setup()
    {
        gameObject.SetActive(true);
        imageComponent = gameObject.GetComponentInChildren<Image>();
        //restartButton = gameObject.GetComponentInChildren<Button>();

        if (imageComponent != null)
        {
            StartCoroutine(FadeImage(imageComponent, 0.5f));
            StartCoroutine(FadeImage(restartButton, 0.2f));
            StartCoroutine(FadeImage(MainMenuButton, 0.2f));
        }

        // 버튼 컴포넌트에 대한 처리도 여기에 추가 가능
    }

    IEnumerator FadeImage(Image targetImage, float fadeSpeed)
    {
        Color currentColor = targetImage.color;
        float currentAlpha = currentColor.a;

        while (currentAlpha < targetAlpha)
        {
            currentAlpha += fadeSpeed * Time.deltaTime;
            targetImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentAlpha);
            yield return null;
        }
        // 불투명도가 목표값에 도달했을 때 추가 작업 가능
    }

    public void ExitButton()
    {
        Time.timeScale = 1;
        Loading.LoadScene(0);
    }
    public void RestartButton()
    {
        Loading.LoadScene(1);
    }
}
