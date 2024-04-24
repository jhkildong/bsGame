using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Yeon2;

public class ForwardWeaponBOTCA : Bless
{

    /*
    1레벨 - 전방으로 1개 발사
    2레벨 - 전방으로 2개 발사
    3레벨 - 대미지 20% 증가
    4레벨 - 전방으로 3개 발사
    5레벨 - 공격속도 50% 증가
    6레벨 - 전방으로 4개 발사
    7레벨 - 전방으로 5개 발사
    */

    public GameObject weaponPrefab; // 생성할 프리팹
    public Transform clonesParent; // 생성한 프리펩들 보관할 곳

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

    public void OnOkSpawnForwardWeapon() // 클릭시 호출
    {
        if(Level < 7)
        {
            Level++;
            LevelUp(Level);
            Debug.Log($"{Level}Level 입니다.");
        }
    }

    private void SpawnWeapon()
    {
        GameObject go = Instantiate(weaponPrefab, transform.position, transform.rotation); // 무기 생성
        go.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]); //사이즈
        go.transform.SetParent(clonesParent); // 생성한 무기 똥처리
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