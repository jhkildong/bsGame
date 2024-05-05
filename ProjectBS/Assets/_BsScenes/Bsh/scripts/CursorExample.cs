using UnityEngine;

public class CursorExample : MonoBehaviour
{
    public Texture2D StartScene; // Ŀ���� ����� �ؽ�ó
    public Texture2D InGame;

    void Start()
    {
        Cursor.SetCursor(StartScene, Vector2.zero, CursorMode.Auto);
    }
}
