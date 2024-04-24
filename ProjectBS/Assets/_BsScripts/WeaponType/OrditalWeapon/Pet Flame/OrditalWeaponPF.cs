using UnityEngine;
using Yeon2;

public class OrditalWeaponPF : Bless
{
    public static Quaternion myRotation; // ȸ����

    public GameObject weaponBowPrefab; // Ȱ ������ ������
    public GameObject weaponArrowPrefab; // ȭ�� ������ ������

    public float RotSpeed = 100.0f;
    public float size = 2.0f;
    public float AtRange = 1.0f;
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
        transform.position = myPlayer.transform.position + new Vector3(0, 0.7f, 0);
        myRotation = myPlayer.transform.rotation;
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

        // ������ ���� ������ �����ϰ� ����.
        int childCount = transform.childCount;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
            child.position = transform.position + direction * AtRange;
        }

        var Bow = go.GetComponent<OrditalWeaponPF_Bow>();
        Bow.ReTime = myStatus[Key.ReTime];
        Bow.ArrowAmount = myStatus[Key.ArrowAmount];

        var Arrow = weaponArrowPrefab.GetComponentInChildren<OrditalWeaponPF_Arrow>();
        Arrow.Ak = myStatus[Key.Attack];
    }
}