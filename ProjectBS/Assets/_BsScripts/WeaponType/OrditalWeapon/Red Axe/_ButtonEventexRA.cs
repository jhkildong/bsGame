using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _ButtonEventexRA : MonoBehaviour
{
    //��ư �����鿡 �־�� �Ǵ� ��ũ��.
    private Button button;
    private OrditalWeaponRA Weapon;
    // Start is called before the first frame update
    private void Start()
    {
        button = GetComponent<Button>(); //��ư component ��������
        button.onClick.AddListener(OnButtonClick); //���ڰ� ���� �� �Լ� ȣ��
        Weapon = FindObjectOfType<OrditalWeaponRA>();
    }

    private void OnButtonClick()
    {
        Weapon.OnOkSpawnOrditalWeapon(); // OrditalWeapon�� OnOkSpawnOrditalWeapon() �޼��� ȣ��
    }
}
