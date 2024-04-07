using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _ButtonEventexBOL : MonoBehaviour
{
    //��ư �����鿡 �־�� �Ǵ� ��ũ��.
    private Button button;
    private RangeWeaponBOL Weapon;

    // Start is called before the first frame update
    private void Start()
    {
        button = GetComponent<Button>(); //��ư component ��������
        button.onClick.AddListener(OnButtonClick); //���ڰ� ���� �� �Լ� ȣ��
        Weapon = FindObjectOfType<RangeWeaponBOL>();
    }

    private void OnButtonClick()
    {
        Weapon.OnOkSpawnRangeWaepon(); // OrditalWeapon�� OnOkSpawnOrditalWeapon() �޼��� ȣ��
    }
}
