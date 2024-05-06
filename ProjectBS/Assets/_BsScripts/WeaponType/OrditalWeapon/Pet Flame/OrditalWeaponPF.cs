using System.Collections.Generic;
using UnityEngine;

public class OrditalWeaponPF : Bless
{
    public static Quaternion myRotation; // 회전값

    public GameObject weaponBowPrefab; // 활 생성한 프리팹
    private List<GameObject> myBows; // 생성한 활들

    float RotSpeed = 100.0f;
    float AtRange = 2.0f;

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
        if (rotatingBody == null) return;
        myRotation = rotatingBody.rotation;
        transform.Rotate(Vector3.up, -RotSpeed * Time.deltaTime); // 공전

        if (CurLv >= 0)
        {
            if (Count < myStatus[Key.Amount])
            {
                Count++;
                SpawnWeaponBow();
            }
        }
    }


    private void SpawnWeaponBow() // 활 생성
    {
        GameObject go = Instantiate(weaponBowPrefab, transform); // 무기 생성
        myBows.Add(go);
        int childCount = myBows.Count;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            // 생성한 무기 간격을 일정하게 맞춤.
            Transform child = myBows[i].transform;
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
            child.position = transform.position + direction * AtRange;

            // 생성한 무기들 값 대입
            var Bow = child.GetComponent<OrditalWeaponPF_Bow>();
            Bow.ReTime = myStatus[Key.ReTime];
            Bow.ArrowAmount = myStatus[Key.ArrowAmount];
        }
    }
}