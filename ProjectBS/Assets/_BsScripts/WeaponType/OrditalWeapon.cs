using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrditalWeapon : MonoBehaviour
{
    public Transform myTarget; // 회전 중심점
    public GameObject weaponPrefabs;
    public float rotSpeed = 30.0f; // 공전 속도
    
    short Count = 0;
    private GameObject spawnedWeapon; // 생성된 무기를 저장할 변수

    // Start is called before the first frame update
    void Start()
    {
        Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = new Vector3(myTarget.position.x, transform.position.y, myTarget.position.z);
        transform.position = targetPos; // 자신의 위치
        transform.RotateAround(myTarget.position, Vector3.up, rotSpeed * Time.deltaTime); // 공전

    }

    public void SpawnOrditalWeapon()
    {
        if (Count < 7)
        {
            Count++;
            Vector3 spawnPostion = transform.position + transform.forward;

            GameObject newWeapon = Instantiate(weaponPrefabs, spawnPostion, Quaternion.identity);
            newWeapon.transform.SetParent(transform);

            int childCount = transform.childCount;
            float angleStep = 360.0f / childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
                child.position = transform.position + direction * 2.0f;
            }
        }
    }
}
