using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
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
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/BuildingPopup"), this.transform);
            }
        }
    }
}
