using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OrditalWeaponPF_Bow : MonoBehaviour
{
    public GameObject weaponArrowPrefab; // 생성한 프리펩

    float reTime; // 공격속도
    float waitTime; // 재 발사 시간
    float destroyTime; // 생성한 무기 없는 시간
    short arrowAmount; // 화살 갯수
    float size;

    float time = 0;

    private void OnEnable()
    {
        OrditalWeaponPF orditalWeaponPF = FindObjectOfType<OrditalWeaponPF>();
        if (orditalWeaponPF != null)
        {
            reTime = orditalWeaponPF.ReTime;
            waitTime = orditalWeaponPF.WaitTime;
            destroyTime = orditalWeaponPF.DestroyTime;
            arrowAmount = orditalWeaponPF.ArrowAmount;
            size = orditalWeaponPF.Size;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Quaternion rotation = ForwardWeaponLD.myRotation;
        transform.rotation = rotation;
        if (time >= reTime)
        {
            time = 0.0f;
            StartCoroutine(SpawnMultipleWeapons(arrowAmount));
        }
    }

    private void SpawnWeaponArrow() // 화살 생성
    {
        GameObject bullet = Instantiate(weaponArrowPrefab, transform.position, transform.rotation); // 무기 생성
        bullet.transform.localScale = new Vector3(size, size, size);
        bullet.transform.SetParent(null); // 생성 무기 똥처리
        Destroy(bullet, destroyTime);
    }
    IEnumerator SpawnMultipleWeapons(int v)
    {
        for (int i = 0; i < v; i++)
        {
            SpawnWeaponArrow();
            yield return new WaitForSeconds(waitTime);
        }
    }
}
