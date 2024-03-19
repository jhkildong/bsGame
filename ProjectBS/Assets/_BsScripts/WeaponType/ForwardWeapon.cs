using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ForwardWeapon : MonoBehaviour
{
    public Transform myTarget;
    public GameObject objectPrefab; // 생성할 물체의 프리팹

    public float bulletSpeed = 5.0f;

    float time = 0.0f;
    float reTime = 2.0f;
    short Count = 0;
    // Start is called before the first frame update
    void Start()
    {
        Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 자신의 위치(x,z 값만 받음)
        Vector3 targetPos = new Vector3(myTarget.position.x, transform.position.y, myTarget.position.z);
        transform.position = targetPos;

        // 회전축을 같게 만듬.
        transform.rotation = myTarget.rotation;
        time += Time.deltaTime;

        if (Count > 0)
        {
            if (time >= reTime)
            {
                GameObject bullet = Instantiate(objectPrefab, transform.position, transform.rotation);
                bullet.transform.SetParent(null);
                Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
                Debug.Log($"{reTime}초가 지났습니다.");
                time = 0f;
            }
        }
    }

    public void SpawnForwardWeapon()
    {
        if(Count < 7)
        {
            Count++;
            if (Count > 1)
            {
                reTime *= 0.8f;
            }
        }
    }

}