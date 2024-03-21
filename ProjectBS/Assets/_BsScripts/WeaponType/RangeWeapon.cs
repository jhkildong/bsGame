using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    public Transform myTarget;
    public GameObject objectPrefab;
    public float atRange = 3.0f; // 생성반경
    public float reTime = 2.0f; // 재생성 시간
    public float desTime = 3.0f; // 없어지는 시간

    short Count = 0;
    float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (myTarget != null) transform.SetParent(myTarget);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (Count > 0)
        {
            if (time >= reTime)
            {
                Vector3 randomPos = Random.insideUnitSphere * atRange;
                randomPos.y = 0.0f;
                GameObject bullet = Instantiate(objectPrefab, randomPos, Quaternion.identity);
                Debug.Log($"{reTime}초가 지났습니다.");
                Destroy(bullet, desTime);
                time = 0f;
            }
        }
    }

    public void SpawnRangeWaepon()
    {
        if (Count < 7)
        {
            Count++;
            if (Count > 1) reTime *= 0.8f; // 20퍼
        }
    }
}
