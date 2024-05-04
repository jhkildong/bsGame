using System.Collections;
using UnityEngine;

public class ForwardWeaponCS : Bless
{
    public float AtRange { get => _atRange; set => _atRange = value; }
    public float MaxRange { get => _maxRange; set => _maxRange = value; }
    public float MinRange { get => _minRange; set => _minRange = value; }

    [SerializeField] private float _atRange; // �÷��̾�κ��� ���� �����Ÿ�
    [SerializeField] private float _maxRange; // �ִ� ��
    [SerializeField] private float _minRange; // �ּ� ��

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

        if (CurLv >= 1)
        {
            if (time >= myStatus[Key.ReTime])
            {
                time = 0.0f;
                StartCoroutine(SpawnMultipleWeapons(myStatus[Key.Amount]));
                Debug.Log($"�ð� {myStatus[Key.ReTime]}");
            }
        }
    }

    private void SetSpawnWeapon()
    {
        float randomXPos = Random.Range(MinRange, MaxRange); // ���� �ּ�, �ִ� �� ����

        var bullet = SpawnWeapon() as ForwardMovingWeapon; // ���� ����
        bullet.transform.SetPositionAndRotation(transform.position + Vector3.right * randomXPos + transform.forward * AtRange, transform.rotation);
        bullet.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]); // ������
        
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