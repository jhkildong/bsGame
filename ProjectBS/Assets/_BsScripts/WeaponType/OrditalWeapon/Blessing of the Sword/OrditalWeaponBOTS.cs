using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using Yeon2;

public class OrditalWeaponBOTS : Bless
{
    public GameObject weaponPrefab; // ������ ������

    short Level = 0;
    short Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Level = Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = myPlayer.transform.position + new Vector3(0, 0.5f, 0);
        transform.Rotate(Vector3.up, -myStatus[Key.RotSpeed] * Time.deltaTime); // ����
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
        go.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]);

        // ������ ���� ������ �����ϰ� ����.
        int childCount = transform.childCount;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Vector3 eulerAngle = child.localRotation.eulerAngles;
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
            child.position = transform.position + direction * myStatus[Key.AtRange];
            child.localRotation = Quaternion.Euler(eulerAngle.x, angleStep * i, eulerAngle.z); // ������ rotation�� Ÿ�������� ����.
        }

        var bullet = go.GetComponentInChildren<OrditalWeaponBOTS_Bullet>();
        bullet.Ak = myStatus[Key.Attack];
    }
}
