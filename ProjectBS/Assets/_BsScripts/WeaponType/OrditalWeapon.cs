using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrditalWeapon : MonoBehaviour
{
    public Transform myTarget; // 회전 중심점
    public GameObject weaponPrefabs;
    public float rotSpeed = 30.0f; // 공전 속도
    public float attakRange = 2.0f;
    
    short Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Count = 0;
        if(myTarget != null) transform.SetParent(myTarget);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime); // 공전
    }

    public void SpawnOrditalWeapon()
    {
        if (Count < 7)
        {
            Count++;
            GameObject newWeapon = Instantiate(weaponPrefabs, transform);

            int childCount = transform.childCount;
            float angleStep = 360.0f / childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
                child.position = transform.position + direction * attakRange;
            }
        }
    }
}
