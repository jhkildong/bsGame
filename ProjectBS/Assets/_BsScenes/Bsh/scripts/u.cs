using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class u : MonoBehaviour
{
    public TMP_Text tMP_Text;
    int Max_Hp, Cur_Hp, MoveSpeed, Attack, Mag;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        tMP_Text.text = $"{Max_Hp}\n{Cur_Hp}\n{MoveSpeed}\n{Attack}\n{Mag}\n{gameManager.CurGold()}\n{gameManager.CurLvExp}";
    }
}