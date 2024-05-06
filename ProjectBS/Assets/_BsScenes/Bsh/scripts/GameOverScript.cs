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
        Setup();
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

        // ��ư ������Ʈ�� ���� ó���� ���⿡ �߰� ����
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
        // �������� ��ǥ���� �������� �� �߰� �۾� ����
    }
    public void RestartButton()
    {
        //SceneManager.LoadScene("LoadingScene");
        Loading.LoadScene(0);
    }

    public void ExitButton()
    {
        //SceneManager.LoadScene("MainMenu1");
        Loading.LoadScene(1);
    }
}
