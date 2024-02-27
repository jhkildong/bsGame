using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydrop : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DropItem;
    public int hp = 10;

    public float highValueItem = 0.5f;
    public float middleValueItem = 7.5f;
    public float lowValueItem;

    public string boolParameter;
    public Animator animator;

    private bool hasDead = false;

    void Start()
    {
        lowValueItem = 100.0f - (highValueItem + middleValueItem);
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0 && !hasDead)
        {
            hasDead = true;
            StartCoroutine(DeathAnimation());
            SpawnItem();
        }
        
    }
    IEnumerator DeathAnimation()
    {
        animator.SetBool(boolParameter, true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        //DestroyObject();
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    void SpawnItem()
    {
        Vector3 rnd = transform.position + Random.insideUnitSphere * 2.0f;
        rnd.y = 1.0f;
        Instantiate(DropItem, rnd, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ºÎµóÇô¼­ Ã¼·ÂÀÌ ±ðÀÓ");
        hp -= 5;
    }
}
