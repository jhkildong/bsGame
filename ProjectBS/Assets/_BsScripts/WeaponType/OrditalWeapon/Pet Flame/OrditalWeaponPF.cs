using UnityEngine;
using Yeon2;

public class OrditalWeaponPF : Bless
{
    public static Quaternion myRotation; // ȸ����

    public GameObject weaponBowPrefab; // Ȱ ������ ������
    public GameObject weaponArrowPrefab; // ȭ�� ������ ������

    float RotSpeed = 100.0f;
    float AtRange = 1.0f;

    short Level = 0;
    short Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        myRotation = rotatingBody.rotation;
        transform.Rotate(Vector3.up, -RotSpeed * Time.deltaTime); // ����

        if (Level >= 1)
        {
            if (Count < myStatus[Key.Amount])
            {
                Count++;
                SpawnWeaponBow();
            }
        }
    }

    public void OnOkSpawnOrditalWeapon()
    {
        if (Level < 7)
        {
            Level++;
            LevelUp(Level);
            Debug.Log($"{Level}Level �Դϴ�.");
        }
    }


    private void SpawnWeaponBow() // Ȱ ����
    {
        GameObject go = Instantiate(weaponBowPrefab, transform); // ���� ����

        int childCount = transform.childCount;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            // ������ ���� ������ �����ϰ� ����.
            Transform child = transform.GetChild(i);
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
            child.position = transform.position + direction * AtRange;

            // ������ ����� �� ����
            var Bow = child.GetComponent<OrditalWeaponPF_Bow>();
            Bow.ReTime = myStatus[Key.ReTime];
            Bow.ArrowAmount = myStatus[Key.ArrowAmount];

            var Arrow = child.GetComponentInChildren<ForwardMovingWeapon>();
            Arrow.Ak = myStatus[Key.Attack];
        }
    }
}