using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartUp : MonoBehaviour
{
    private float originalRadius; // 원래 반지름 값
    public float increaseAmount = 1.0f; // 증가할 양
    // Start is called before the first frame update
    void Start()
    {
        originalRadius = GetComponent<SphereCollider>().radius;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<SphereCollider>().radius += increaseAmount;
        Debug.Log("자성이 올라감.");

        // 일정 시간이 지난 후 반지름을 원래 크기로 돌리는 코루틴 시작
        StartCoroutine(ResetRadius(other.GetComponent<SphereCollider>()));
    }
    private IEnumerator ResetRadius(SphereCollider collider)
    {
        yield return new WaitForSeconds(5.0f); // 5초 후

        // 반지름을 원래 크기로 설정
        collider.radius = originalRadius;
        Debug.Log("자성이 원래대로 돌아옴.");
    }
}
