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
    public Transform myTarget; // 따라갈 타겟
    public GameObject weaponPrefab; // 생성할 프리팹
    public Transform clonesParent; // 생성한 프리펩들 보관할 곳

    public float ReTime { get => _reTime; set => _reTime = value; }
    public float WaitTime { get => _waitTime; set => _waitTime = value; }
    public float DestroyTime { get => _destroyTime; set => _destroyTime = value; }

    [SerializeField] private float _reTime; // 공격속도
    [SerializeField] private float _waitTime; // 다음 프리펩 재생성시간
    [SerializeField] private float _destroyTime; // 생성한 무기 없는 시간

    float time = 0.0f;
    short Level = 0;
    short Count = 0;

    float Attack;
    float Amount;


    // Start is called before the first frame update
    void Start()
    {
        Level = Count = 0;
        if (myTarget != null) transform.SetParent(myTarget); // 플레이어에 부착

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

    public void OnOkSpawnForwardWeapon() // 클릭시 호출
    {
        if(Level < 7)
        {
            Level++;
            LevelUp(Level);
            Count = 0;
            Debug.Log($"{Level}Level 입니다.");
        }
    }

    private void SpawnWeapon()
    {
        GameObject go = Instantiate(weaponPrefab, transform.position, transform.rotation); // 무기 생성
        var bullet = go.GetComponent<ForwardWeaponBOTCA_Bullet>();
        bullet.Ak = myStatus[Key.Attack];
        //bullet.transform.localScale = new Vector3(Size, Size, Size);
        go.transform.SetParent(clonesParent); // 생성한 무기 똥처리
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