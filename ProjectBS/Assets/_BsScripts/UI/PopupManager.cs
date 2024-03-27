using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopupManager : MonoBehaviour
{
    public GameObject popup;
    public static short buildingPopupCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            buildingPopupCount++;
            if(buildingPopupCount == 1)
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popup"), this.transform);
            }
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            DestroyAllChildren();
            buildingPopupCount = 0;
        }
    }

    void DestroyAllChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
