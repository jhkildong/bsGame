using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollow : MonoBehaviour
{
    public void Follow(Transform target)
    {
        Debug.Log("Item is following...");
        StartCoroutine(Following(target));
    }
    IEnumerator Following(Transform target)
    {
        Vector3 dir;
        float accel = 0;
        while (target != null)
        {
            dir = target.position - transform.position;
            accel += Time.deltaTime;
            transform.position += dir.normalized * accel;
            if (Vector3.Distance(target.position, transform.position) < 0.25f)
            {
                Eat();  
                yield break;
            }
            yield return null;
        }
    }
    void Eat()
    {
        Destroy(gameObject);
        Debug.Log(gameObject + "을(를) 얻었다..!");
    }
}
