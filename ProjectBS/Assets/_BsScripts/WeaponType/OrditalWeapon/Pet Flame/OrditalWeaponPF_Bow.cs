using System.Collections;
using UnityEngine;

public class OrditalWeaponPF_Bow : MonoBehaviour
{
    public GameObject weaponArrowPrefab; // 积己茄 橇府崎

    public float ReTime; // 傍拜加档
    public float ArrowAmount; // 拳混 肮荐

    public float Size = 2.0f;

    float time = 0;
    float WaitTime = 0.05f;
    float DestroyTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

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

    private void SpawnWeaponArrow() // 拳混 积己
    {
        GameObject go = Instantiate(weaponArrowPrefab, transform.position, transform.rotation); // 公扁 积己
        go.transform.localScale = new Vector3(Size, Size, Size);

        go.transform.SetParent(null); // 积己 公扁 端贸府
        Destroy(go, DestroyTime);
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
