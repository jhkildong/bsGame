using System.Collections;
using UnityEngine;

public class ForwardWeaponCS : Bless
{
    public float AtRange { get => _atRange; set => _atRange = value; }
    public float MaxRange { get => _maxRange; set => _maxRange = value; }
    public float MinRange { get => _minRange; set => _minRange = value; }

    [SerializeField] private float _atRange; // 플레이어로부터 무기 생성거리
    [SerializeField] private float _maxRange; // 최대 폭
    [SerializeField] private float _minRange; // 최소 폭

    float time = 0.0f;
    float WaitTime = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        SetFowardPlayerLook();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (CurLv >= 0)
        {
            if (time >= myStatus[Key.ReTime])
            {
                time = 0.0f;
                StartCoroutine(SpawnMultipleWeapons(myStatus[Key.Amount]));
                Debug.Log($"시간 {myStatus[Key.ReTime]}");
            }
        }
    }

    private void SetSpawnWeapon()
    {
        float randomXPos = Random.Range(MinRange, MaxRange); // 랜덤 최소, 최대 폭 설정

        var bullet = SpawnWeapon() as ForwardMovingWeapon; // 무기 생성
        bullet.transform.SetPositionAndRotation(transform.position + Vector3.right * randomXPos + transform.forward * AtRange, transform.rotation);
        bullet.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]); // 사이즈
        
        bullet.Ak = myStatus[Key.Attack];
        bullet.Shoot(25);
    }

    IEnumerator SpawnMultipleWeapons(float v)
    {
        for (int i = 0; i < v; i++)
        {
            SetSpawnWeapon();
            yield return new WaitForSeconds(WaitTime);
        }
    }

}