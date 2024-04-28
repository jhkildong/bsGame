using System.Collections;
using UnityEngine;
using Yeon2;

public class ForwardWeaponCS : Bless
{
    public GameObject weaponPrefab; // ������ ������

    public float AtRange { get => _atRange; set => _atRange = value; }
    public float MaxRange { get => _maxRange; set => _maxRange = value; }
    public float MinRange { get => _minRange; set => _minRange = value; }

    [SerializeField] private float _atRange; // �÷��̾�κ��� ���� �����Ÿ�
    [SerializeField] private float _maxRange; // �ִ� ��
    [SerializeField] private float _minRange; // �ּ� ��

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
                Debug.Log($"�ð� {myStatus[Key.ReTime]}");
            }
        }
    }

    public void OnOkSpawnForwardWeapon()
    {
        if (Level < 7)
        {
            Level++;
            LevelUp(Level);
            Debug.Log($"{Level}Level �Դϴ�.");
        }
    }

    private void SpawnWeapon()
    {
        Vector3 direction = Quaternion.Euler(0, 0, 0) * transform.forward;
        float randomXPos = Random.Range(MinRange, MaxRange); // ���� �ּ�, �ִ� �� ����

        GameObject go = Instantiate(weaponPrefab, transform.position, transform.rotation); // ���� ����
        go.transform.position = transform.position + new Vector3(randomXPos, 0.7f, 0.0f) + direction * AtRange; // �Ÿ��� �� ����
        go.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]); // ������

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