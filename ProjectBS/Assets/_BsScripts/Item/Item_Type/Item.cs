using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(ItemFollow))]
public abstract class Item : MonoBehaviour, IPoolable
{
    public int ID => Data.ID;
    public MonoBehaviour This => this;
    public IPoolable CreateClone()
    {
        return Data.CreateItem();
    }
    public ItemData Data { get; private set; }
    
    private Collider col;

    private void Awake()
    {
        if (!TryGetComponent(out col))
        {
            col = gameObject.AddComponent<SphereCollider>();
        }
        col.isTrigger = true;
    }

    public virtual void Init(ItemData data)
    {
        Instantiate(data.ItemPrefab, transform);

        Data = data;
        gameObject.layer = (int)(BSLayers.Item);
    }


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

    protected virtual void Eat()
    {
        ObjectPoolManager.Instance.ReleaseObj(this);
        Debug.Log(gameObject + "을(를) 얻었다..!");
    }
}
