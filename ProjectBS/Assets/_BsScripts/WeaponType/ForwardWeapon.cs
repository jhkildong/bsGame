using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ForwardWeapon : MonoBehaviour
{
    public Transform myTarget;
    public Transform myRotation;
    public GameObject objectPrefab; // ������ ��ü�� ������

    float time = 0.0f; // �ð� ���� ����
    float reTime = 2.0f; // ���� �߻� �ð� ����
    short Count = 0;
    // Start is called before the first frame update
    void Start()
    {
        Count = 0;
        if (myTarget != null) transform.SetParent(myTarget);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = myRotation.rotation;
        time += Time.deltaTime;

        if (Count > 0)
        {
            if (time >= reTime)
            {
                GameObject swordBullet = Instantiate(objectPrefab, transform.position , transform.rotation);
                swordBullet.transform.SetParent(null);
                Rigidbody SwordRigidbody = swordBullet.GetComponent<Rigidbody>();
                Debug.Log($"{reTime}�ʰ� �������ϴ�.");
                time = 0f;
            }
        }
    }

    public void SpawnForwardWeapon()
    {
        if(Count < 7)
        {
            Count++;
            if (Count > 1) reTime *= 0.8f;
        }
    }

}