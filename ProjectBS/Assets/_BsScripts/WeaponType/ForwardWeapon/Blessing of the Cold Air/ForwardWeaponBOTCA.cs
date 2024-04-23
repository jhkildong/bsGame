using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Yeon2;

/*Transform tr;
Transform PlayerT = transform;
tr = PlayerT.GetComponentInChildren<PlayerComponent>().MyTransform;

this.transform.position = tr.position;
this.transform.rotation = tr.rotation;*/

public class ForwardWeaponBOTCA : Bless
{
    public Transform myTarget; // ���� Ÿ��
    public GameObject weaponPrefab; // ������ ������
    public Transform clonesParent; // ������ ������� ������ ��

    public float ReTime { get => _reTime; set => _reTime = value; }
    public float WaitTime { get => _waitTime; set => _waitTime = value; }
    public float DestroyTime { get => _destroyTime; set => _destroyTime = value; }

    [SerializeField] private float _reTime; // ���ݼӵ�
    [SerializeField] private float _waitTime; // ���� ������ ������ð�
    [SerializeField] private float _destroyTime; // ������ ���� ���� �ð�

    float time = 0.0f;
    short Level = 0;
    short Count = 0;

    float Attack;
    float Amount;


    // Start is called before the first frame update
    void Start()
    {
        Level = Count = 0;
        if (myTarget != null) transform.SetParent(myTarget); // �÷��̾ ����

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = myPlayer.transform.rotation;
        time += Time.deltaTime;
        if (time >= ReTime)
        {
            time = 0.0f;
            StartCoroutine(SpawnMultipleWeapons(myStatus[Key.Amount]));
        }
    }

    public void OnOkSpawnForwardWeapon() // Ŭ���� ȣ��
    {
        if(Level < 7)
        {
            Level++;
            LevelUp(Level);
            Count = 0;
            Debug.Log($"{Level}Level �Դϴ�.");
        }
    }

    private void SpawnWeapon()
    {
        GameObject go = Instantiate(weaponPrefab, transform.position, transform.rotation); // ���� ����
        var bullet = go.GetComponent<ForwardWeaponBOTCA_Bullet>();
        bullet.Ak = myStatus[Key.Attack];
        //bullet.transform.localScale = new Vector3(Size, Size, Size);
        go.transform.SetParent(clonesParent); // ������ ���� ��ó��
        Destroy(go, DestroyTime);
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