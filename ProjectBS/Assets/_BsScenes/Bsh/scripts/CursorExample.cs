using UnityEngine;

public class CursorExample : MonoBehaviour
{
    public Texture2D cursorTexture; // Ŀ���� ����� �ؽ�ó

    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}
