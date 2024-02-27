using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public int cost;
    public string name;
    public int HeightOfItem = 1;
    public GameObject itemPrefab;
    public LayerMask itemMask;
    public float spawnRadius;

    private GameObject currentItem;
    // Start is called before the first frame update
    void Start()
    {
        //SpawnItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("æ∆¿Ã≈€ ∏‘¿Ω!");
        Destroy(gameObject);
        DestroyItem();
        SpawnItem();
    }

    void SpawnItem()
    {
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = HeightOfItem;
        currentItem = Instantiate(itemPrefab, randomPosition, Quaternion.identity);
    }

    void DestroyItem()
    {
        Destroy(itemPrefab);
    }
}
