using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _ButtonEventexBOTCA: MonoBehaviour
{
    //��ư �����鿡 �־�� �Ǵ� ��ũ��.
    private Button button;
    private ForwardWeaponBOTCA Weapon;
    // Start is called before the first frame update
    private void Start()
    {
        button = GetComponent<Button>(); //��ư component ��������
        button.onClick.AddListener(OnButtonClick); //���ڰ� ���� �� �Լ� ȣ��
        Weapon = BlessManager.Instance.CreateBless(BlessID.BOTCA) as ForwardWeaponBOTCA;
    }

    private void OnButtonClick()
    {
        Weapon.LevelUp();
    }
}
