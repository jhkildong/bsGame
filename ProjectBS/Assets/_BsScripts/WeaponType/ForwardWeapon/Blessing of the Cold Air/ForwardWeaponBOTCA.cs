using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Yeon2;

public class ForwardWeaponBOTCA : Bless
{

    /*
    1���� - �������� 1�� �߻�
    2���� - �������� 2�� �߻�
    3���� - ����� 20% ����
    4���� - �������� 3�� �߻�
    5���� - ���ݼӵ� 50% ����
    6���� - �������� 4�� �߻�
    7���� - �������� 5�� �߻�
    */

    public GameObject weaponPrefab; // ������ ������
    public Transform clonesParent; // ������ ������� ������ ��

    float time = 0.0f;
    short Level = 0;
    float WaitTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = myPlayer.transform.position + new Vector3(0, 0.5f, 0);
        transform.rotation = myPlayer.transform.rotation;
        time += Time.deltaTime;
        if (Level >= 1)
        {
            //Debug.Log(myStatus[Key.Amount]);
            if (time >= myStatus[Key.ReTime])
            {
                time = 0.0f;
                StartCoroutine(SpawnMultipleWeapons(myStatus[Key.Amount]));

            }
        }

    }

    public void OnOkSpawnForwardWeapon() // Ŭ���� ȣ��
    {
        if(Level < 7)
        {
            Level++;
            LevelUp(Level);
            Debug.Log($"{Level}Level �Դϴ�.");
        }
    }

    private void SpawnWeapon()
    {
        GameObject go = Instantiate(weaponPrefab, transform.position, transform.rotation); // ���� ����
        go.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]); //������
        go.transform.SetParent(clonesParent); // ������ ���� ��ó��
        Destroy(go, myStatus[Key.DestroyTime]);

        var bullet = go.GetComponent<ForwardWeaponBOTCA_Bullet>();
        bullet.Ak = myStatus[Key.Attack];
    }
    IEnumerator SpawnMultipleWeapons(float v)
    {
        for (int i = 0; i < v; i++)
        {
            SpawnWeapon();
            yield return new WaitForSeconds(0.05f);
        }
    }

}