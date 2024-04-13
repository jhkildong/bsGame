using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _ButtonEventexVG: MonoBehaviour
{
    //��ư �����鿡 �־�� �Ǵ� ��ũ��.
    private Button button;
    private ForwardWeaponVG Weapon;
    // Start is called before the first frame update
    private void Start()
    {
        button = GetComponent<Button>(); //��ư component ��������
        button.onClick.AddListener(OnButtonClick); //���ڰ� ���� �� �Լ� ȣ��
        Weapon = FindObjectOfType<ForwardWeaponVG>();
    }

    private void OnButtonClick()
    {
        Weapon.OnOkSpawnForwardWeapon();
    }
}
