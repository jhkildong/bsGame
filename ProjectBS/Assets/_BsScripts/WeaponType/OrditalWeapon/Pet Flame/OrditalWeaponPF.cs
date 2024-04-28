using UnityEngine;
using Yeon2;

public class OrditalWeaponPF : Bless
{
    public static Quaternion myRotation; // 회전값

    public GameObject weaponBowPrefab; // 활 생성한 프리펩
    public GameObject weaponArrowPrefab; // 화살 생성한 프리펩

    float RotSpeed = 100.0f;
    float AtRange = 1.0f;

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
        myRotation = rotatingBody.rotation;
        transform.Rotate(Vector3.up, -RotSpeed * Time.deltaTime); // 공전

        if (Level >= 1)
        {
            if (Count < myStatus[Key.Amount])
            {
                Count++;
                SpawnWeaponBow();
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


    private void SpawnWeaponBow() // 활 생성
    {
        GameObject go = Instantiate(weaponBowPrefab, transform); // 무기 생성

        int childCount = transform.childCount;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            // 생성한 무기 간격을 일정하게 맞춤.
            Transform child = transform.GetChild(i);
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
            child.position = transform.position + direction * AtRange;

            // 생성한 무기들 값 대입
            var Bow = child.GetComponent<OrditalWeaponPF_Bow>();
            Bow.ReTime = myStatus[Key.ReTime];
            Bow.ArrowAmount = myStatus[Key.ArrowAmount];

            var Arrow = child.GetComponentInChildren<ForwardMovingWeapon>();
            Arrow.Ak = myStatus[Key.Attack];
        }
    }
}