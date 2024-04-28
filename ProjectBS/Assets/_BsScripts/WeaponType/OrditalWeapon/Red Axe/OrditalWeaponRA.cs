using UnityEngine;
using Yeon2;

public class OrditalWeaponRA : Bless
{
    public GameObject weaponPrefab; // ���� ������

    short Level = 0;
    short Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, myStatus[Key.RotSpeed] * Time.deltaTime); // ����

        if (Level >= 1)
        {
            if (Count < myStatus[Key.Amount])
            {
                Count++;
                SpawnWeapon();
            }
        }
    }

    public void OnOkSpawnOrditalWeapon()
    {
        if (Level < 7)
        {
            Level++;
            LevelUp(Level);
            Debug.Log($"{Level}Level �Դϴ�.");
        }
    }

    private void SpawnWeapon()
    {
        GameObject go = Instantiate(weaponPrefab, transform); // ���� ����

        int childCount = transform.childCount;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]); // ������
            Vector3 eulerAngle = child.localRotation.eulerAngles;
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward; // ������ ���� ������ �����ϰ� ����.
            child.position = transform.position + direction * myStatus[Key.AtRange]; // �Ÿ�
            child.localRotation = Quaternion.Euler(eulerAngle.x, angleStep * i, eulerAngle.z); // ������ rotation�� Ÿ�������� ����.

            // ����� �� ����
            var bullet = child.GetComponentInChildren<Weapon>();
            bullet.Ak = myStatus[Key.Attack];

        }        
    }
}
