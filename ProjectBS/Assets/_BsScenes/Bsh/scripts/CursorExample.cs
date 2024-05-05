using UnityEngine;

public class CursorExample : MonoBehaviour
{
    public Texture2D StartScene; // 커서로 사용할 텍스처
    public Texture2D InGame;

    void Start()
    {
        Cursor.SetCursor(StartScene, Vector2.zero, CursorMode.Auto);
    }
}
