using UnityEngine;
using Yeon2;

public class OrditalWeaponRA : Bless
{
    public GameObject weaponPrefab; // 무기 프리펩

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
        transform.Rotate(Vector3.up, myStatus[Key.RotSpeed] * Time.deltaTime); // 공전

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

        int childCount = transform.childCount;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]); // 사이즈
            Vector3 eulerAngle = child.localRotation.eulerAngles;
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward; // 생성한 무기 간격을 일정하게 맞춤.
            child.position = transform.position + direction * myStatus[Key.AtRange]; // 거리
            child.localRotation = Quaternion.Euler(eulerAngle.x, angleStep * i, eulerAngle.z); // 프리팹 rotation을 타켓쪽으로 맞춤.

            // 무기들 값 대입
            var bullet = child.GetComponentInChildren<Weapon>();
            bullet.Ak = myStatus[Key.Attack];

        }        
    }
}
