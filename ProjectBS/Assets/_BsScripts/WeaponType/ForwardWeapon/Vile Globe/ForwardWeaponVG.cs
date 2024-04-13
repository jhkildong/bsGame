using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class ForwardWeaponVG : MonoBehaviour
{
    public Transform myTarget; // ���� Ÿ��
    public Transform myRotate; // ���� ȸ���� Ÿ��
    public GameObject weaponPrefab; // ������ ������

    public float maxRange = 1.0f; // �ִ� ��
    public float minRange = -1.0f; // �ּ� ��

    public float prefabScale = 1.0f; // ������ ũ��
    public float destroyTime = 0.5f; // ������ ���� ���� �ð�
    public float atRange = 5.0f; // �÷��̾�κ��� ���� �����Ÿ�

    public float reTime = 2.0f; // ���ݼӵ�
    public float waitTime = 0.05f; // �� ���� ����

    float time = 0.0f; // �ð� ���� ����

    public short Level = 0;
    short weaponCount = 0; // ���� ����
    short Count = 0; // �ܼ� �ݺ� Ƚ��

    // Start is called before the first frame update
    void Start()
    {
        Level = weaponCount = Count = 0;
        if (myTarget != null) transform.SetParent(myTarget);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = myRotate.rotation;
        time += Time.deltaTime;
        SwitchUpdate();
    }

    public void OnOkSpawnForwardWeapon()
    {
        if (Level < 7)
        {
            Level++;
            Count = 0;
            Debug.Log($"{Level}Level �Դϴ�.");
        }
    }

    void SwitchUpdate()
    {
        switch (Level)
        {
            case 1:  //  1�� ����
                if (time >= reTime)
                {
                    time = 0.0f;
                    SpawnWeapon();
                }
                break;
            case 2:  //  2���� �ȴ�.
                if (Count < 2)
                {
                    Count++;
                    weaponCount++;
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 3:  //  ����� 20% ����
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 20% ����");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 4:  //  3���� �ȴ�.
                if (Count < 1)
                {
                    Count++;
                    weaponCount++;
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 5:  //  ���ݼӵ� 50% ����
                if (Count < 1)
                {
                    Count++;
                    reTime -= reTime * 0.5f;
                    Debug.Log("���ݼӵ� 50% ����");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 6:  //  4���� �ȴ�.
                if (Count < 1)
                {
                    Count++;
                    weaponCount++;
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 7:  //  5���� �ȴ�.
                if (Count < 1)
                {
                    Count++;
                    weaponCount++;
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;

        }
    }

    IEnumerator SpawnMultipleWeapons(int v)
    {
        for (int i = 0; i < v; i++)
        {
            SpawnWeapon();
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnWeapon()
    {

        GameObject bulletVG = Instantiate(weaponPrefab, transform); // ���� ����
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            Vector3 direction = Quaternion.Euler(0.0f, 0.0f, 0.0f) * transform.forward;
            float randomXPos = Random.Range(minRange, maxRange); // ���� �ּ�, �ִ� �� ����
            child.position = transform.position + new Vector3(randomXPos, 0.0f, 0.0f) + direction * atRange;

            child.localScale = new Vector3(prefabScale, prefabScale, prefabScale);
        }
        bulletVG.transform.SetParent(null);
        Destroy(bulletVG, destroyTime);

    }
}