using UnityEngine;
using UnityEngine.UI;

public class u : MonoBehaviour
{
    public Text attackPowerText;

    void Start()
    {
        UpdateAttackPower(1);
    }

    public void UpdateAttackPower(int newAttackPower)
    {
        attackPowerText.text = "���ݷ�: " + newAttackPower.ToString();
    }
}