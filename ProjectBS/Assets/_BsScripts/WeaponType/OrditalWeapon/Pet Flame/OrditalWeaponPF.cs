using System.Collections.Generic;
using UnityEngine;

public class OrditalWeaponPF : Bless
{
    public static Quaternion myRotation; // ȸ����

    public GameObject weaponBowPrefab; // Ȱ ������ ������
    private List<GameObject> myBows; // ������ Ȱ��

    float RotSpeed = 100.0f;
    float AtRange = 1.0f;

    short Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Count = 0;
        myBows = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        myRotation = rotatingBody.rotation;
        transform.Rotate(Vector3.up, -RotSpeed * Time.deltaTime); // ����

        if (CurLv >= 1)
        {
            if (Count < myStatus[Key.Amount])
            {
                Count++;
                SpawnWeaponBow();
            }
        }
    }


    private void SpawnWeaponBow() // Ȱ ����
    {
        GameObject go = Instantiate(weaponBowPrefab, transform); // ���� ����
        myBows.Add(go);
        int childCount = myBows.Count;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            // ������ ���� ������ �����ϰ� ����.
            Transform child = myBows[i].transform;
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
            child.position = transform.position + direction * AtRange;

            // ������ ����� �� ����
            var Bow = child.GetComponent<OrditalWeaponPF_Bow>();
            Bow.ReTime = myStatus[Key.ReTime];
            Bow.ArrowAmount = myStatus[Key.ArrowAmount];
        }
    }
}