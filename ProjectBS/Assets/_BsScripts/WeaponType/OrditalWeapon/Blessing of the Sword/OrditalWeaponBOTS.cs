using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using Yeon2;

public class OrditalWeaponBOTS : Bless
{
    public GameObject weaponPrefab; // 생성한 프리펩

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
        transform.position = myPlayer.transform.position + new Vector3(0.0f, 0.7f, 0.0f);
        transform.Rotate(Vector3.up, -myStatus[Key.RotSpeed] * Time.deltaTime); // 공전
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
            Debug.Log($"{Level}Level 입니다.");
        }
    }

    private void SpawnWeapon()
    {
        GameObject go = Instantiate(weaponPrefab, transform); // 무기 생성

        // 생성한 무기 간격을 일정하게 맞춤.
        int childCount = transform.childCount;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]);
            Vector3 eulerAngle = child.localRotation.eulerAngles;
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
            child.position = transform.position + direction * myStatus[Key.AtRange];
            child.localRotation = Quaternion.Euler(eulerAngle.x, angleStep * i, eulerAngle.z); // 프리팹 rotation을 타켓쪽으로 맞춤.

            var bullet = child.GetComponentInChildren<Weapon>();
            bullet.Ak = myStatus[Key.Attack];

        }
    }
}
