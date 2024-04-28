using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _ButtonEventexLP : MonoBehaviour
{
    //��ư �����鿡 �־�� �Ǵ� ��ũ��.
    private Button button;
    private RangeWeaponLP Weapon;
    // Start is called before the first frame update
    private void Start()
    {
        button = GetComponent<Button>(); //��ư component ��������
        button.onClick.AddListener(OnButtonClick); //���ڰ� ���� �� �Լ� ȣ��
        Weapon = BlessManager.Instance.CreateBless(BlessID.LP) as RangeWeaponLP;
    }

    private void OnButtonClick()
    {
        Weapon.OnOkSpawnRangeWaepon(); // OrditalWeapon�� OnOkSpawnOrditalWeapon() �޼��� ȣ��
    }
}
