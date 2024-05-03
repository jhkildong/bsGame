using System.Collections.Generic;
using UnityEngine;

public class OrditalWeaponBOTS : Bless
{
    short Count = 0;
    private List<Weapon> myChildren;

    // Start is called before the first frame update
    void Start()
    {
        Count = 0;
        myChildren = new List<Weapon>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!myStatus.ContainsKey(Key.RotSpeed)) return;
        transform.Rotate(Vector3.up, -myStatus[Key.RotSpeed] * Time.deltaTime); // ����
        if (CurLv >= 1)
        {
            if (Count < myStatus[Key.Amount])
            {
                Count++;
                SetSpawnWeapon();
            }
        }
    }


    private void SetSpawnWeapon()
    {
        var bullet = SpawnWeapon();
        bullet.transform.SetParent(transform);
        myChildren.Add(bullet);

        int childCount = myChildren.Count;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            var child = myChildren[i];
            child.Ak = myStatus[Key.Attack];
            Transform childTr = child.transform;

            childTr.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]); // ������
            Vector3 eulerAngle = childTr.localRotation.eulerAngles;
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward; // ������ ���� ������ �����ϰ� ����.
            childTr.position = transform.position + direction * myStatus[Key.AtRange]; // �Ÿ�
            childTr.localRotation = Quaternion.Euler(eulerAngle.x, angleStep * i, eulerAngle.z); // ������ rotation�� Ÿ�������� ����.
        }
    }
}
