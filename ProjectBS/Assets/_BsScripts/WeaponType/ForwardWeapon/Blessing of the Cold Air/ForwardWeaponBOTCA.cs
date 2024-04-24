using System.Collections;
using UnityEngine;
using Yeon2;

public class ForwardWeaponBOTCA : Bless
{
    public GameObject weaponPrefab; // ������ ������
    public Transform clonesParent; // ������ ������� ������ ��

    float time = 0.0f;
    short Level = 0;
    float WaitTime = 0.05f;
    float DestroyTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        Level = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = myPlayer.transform.position + new Vector3(0, 0.5f, 0);
        transform.rotation = myPlayer.transform.rotation;
        time += Time.deltaTime;

        if (Level >= 1)
        {
            if (time >= myStatus[Key.ReTime])
            {
                time = 0.0f;
                StartCoroutine(SpawnMultipleWeapons(myStatus[Key.Amount]));
                Debug.Log($"�ð� {myStatus[Key.ReTime]}");
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
        Destroy(go, DestroyTime);

        var bullet = go.GetComponentInChildren<ForwardWeaponBOTCA_Bullet>();
        bullet.Ak = myStatus[Key.Attack];
    }
    IEnumerator SpawnMultipleWeapons(float v)
    {
        for (int i = 0; i < v; i++)
        {
            SpawnWeapon();
            yield return new WaitForSeconds(WaitTime);
        }
    }

}