using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _ButtonEventexRM : MonoBehaviour
{
    //��ư �����鿡 �־�� �Ǵ� ��ũ��.
    private Button button;
    private RangeWeaponRM Weapon;
    // Start is called before the first frame update
    private void Start()
    {
        button = GetComponent<Button>(); //��ư component ��������
        button.onClick.AddListener(OnButtonClick); //���ڰ� ���� �� �Լ� ȣ��
        Weapon = FindObjectOfType<RangeWeaponRM>();
    }

    private void OnButtonClick()
    {
        Weapon.LevelUp(); // OrditalWeapon�� OnOkSpawnOrditalWeapon() �޼��� ȣ��
    }
}
