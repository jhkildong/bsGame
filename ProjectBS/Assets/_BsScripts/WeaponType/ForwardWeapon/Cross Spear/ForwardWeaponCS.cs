using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using Yeon2;

public class ForwardWeaponCS : Bless
{

    /*
    1레벨 - 전방으로 1개 발사
    2레벨 - 대미지 20% 증가
    3레벨 - 전방으로 2개 발사
    4레벨 - 공격속도 30% 증가
    5레벨 - 대미지 20% 증가
    6레벨 - 전방으로 3개 발사
    7레벨 - 공격속도 30% 증가
    */

    public GameObject weaponPrefab; // 생성할 프리팹
    public Transform clonesParent; // 생성한 프리펩들 보관할 곳

    public float AtRange { get => _atRange; set => _atRange = value; }
    public float MaxRange { get => _maxRange; set => _maxRange = value; }
    public float MinRange { get => _minRange; set => _minRange = value; }

    [SerializeField] private float _atRange; // 플레이어로부터 무기 생성거리
    [SerializeField] private float _maxRange; // 최대 폭
    [SerializeField] private float _minRange; // 최소 폭

    float time = 0.0f;
    short Level = 0;
    float WaitTime = 0.05f;

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
        if (time >= myStatus[Key.ReTime])
        {
            time = 0.0f;
            StartCoroutine(SpawnMultipleWeapons(myStatus[Key.Amount]));
        }
    }

    public void OnOkSpawnForwardWeapon()
    {
        if (Level < 7)
        {
            Level++;
            LevelUp(Level);
            Debug.Log($"{Level}Level 입니다.");
        }
    }

    private void SpawnWeapon()
    {
        GameObject go = Instantiate(weaponPrefab, transform.position, transform.rotation); // 무기 생성
        go.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]);
        go.transform.SetParent(null);
        Destroy(go, myStatus[Key.DestroyTime]);

        var bullet = go.GetComponent<ForwardWeaponBOTCA_Bullet>();
        bullet.Ak = myStatus[Key.Attack];

        int childCount = go.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            Vector3 direction = Quaternion.Euler(0, 0, 0) * transform.forward;
            float randomXPos = Random.Range(MinRange, MaxRange); // 랜덤 최소, 최대 폭 설정
            child.position = transform.position + new Vector3(randomXPos, 0.0f, 0.0f) + direction * AtRange;
        }
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