using System.Collections;
using UnityEngine;

public class OrditalWeaponPF_Bow : MonoBehaviour
{
    public ForwardMovingWeapon weaponArrowPrefab; // 생성한 프리펩

    public float ReTime; // 공격속도
    public float ArrowAmount; // 화살 갯수

    public float Size = 2.0f;

    float time = 0;
    float WaitTime = 0.05f;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Quaternion rotation = OrditalWeaponPF.myRotation;
        transform.rotation = rotation;
        if (time >= ReTime)
        {
            time = 0.0f;
            StartCoroutine(SpawnMultipleWeapons(ArrowAmount));
        }
    }

    private void SpawnWeaponArrow() // 화살 생성
    {
        var Arrow = ObjectPoolManager.Instance.GetObj(weaponArrowPrefab) as ForwardMovingWeapon;
        Arrow.transform.SetPositionAndRotation(transform.position, transform.rotation);
        Arrow.transform.localScale = new Vector3(Size, Size, Size);
        Arrow.Shoot(25);
    }
    IEnumerator SpawnMultipleWeapons(float v)
    {
        for (int i = 0; i < v; i++)
        {
            SpawnWeaponArrow();
            yield return new WaitForSeconds(WaitTime);
        }
    }
}
