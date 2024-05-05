using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLoadingImage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)(System.DateTime.Now.Ticks % int.MaxValue));
        int randomIndex = Random.Range(0, transform.childCount);
        Transform randomchild = transform.GetChild(randomIndex);
        randomchild.gameObject.SetActive(true);
    }
}
