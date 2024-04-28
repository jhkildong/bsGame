using System.Collections;
using UnityEngine;
using Yeon2;

public class ForwardWeaponCS : Bless
{
    public GameObject weaponPrefab; // 생성할 프리팹

    public float AtRange { get => _atRange; set => _atRange = value; }
    public float MaxRange { get => _maxRange; set => _maxRange = value; }
    public float MinRange { get => _minRange; set => _minRange = value; }

    [SerializeField] private float _atRange; // 플레이어로부터 무기 생성거리
    [SerializeField] private float _maxRange; // 최대 폭
    [SerializeField] private float _minRange; // 최소 폭

    float time = 0.0f;
    short Level = 0;
    float WaitTime = 0.05f;
    float DestroyTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Level = 0;
        SetFowardPlayerLook();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (Level >= 1)
        {
            if (time >= myStatus[Key.ReTime])
            {
                time = 0.0f;
                StartCoroutine(SpawnMultipleWeapons(myStatus[Key.Amount]));
                Debug.Log($"시간 {myStatus[Key.ReTime]}");
            }
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
        Vector3 direction = Quaternion.Euler(0, 0, 0) * transform.forward;
        float randomXPos = Random.Range(MinRange, MaxRange); // 랜덤 최소, 최대 폭 설정

        GameObject go = Instantiate(weaponPrefab, transform.position, transform.rotation); // 무기 생성
        go.transform.position = transform.position + new Vector3(randomXPos, 0.7f, 0.0f) + direction * AtRange; // 거리랑 폭 설정
        go.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]); // 사이즈

        var bullet = go.GetComponentInChildren<ForwardMovingWeapon>();
        bullet.Ak = myStatus[Key.Attack];
        bullet.Shoot(35);
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