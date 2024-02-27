using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawn(monster));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnemySpawn(GameObject prefab)
    {
        while(true)
        {
            float x = Random.Range(-20f, 20f);
            float y = Random.Range(-20f, 20f);
            Instantiate(prefab, transform.position + new Vector3(x, 0, y), Quaternion.identity);
            yield return new WaitForSeconds(3.0f);
        }
    }
}
