using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingPopup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnOk()
    {
        PopupManager.buildingPopupCount = 0;
        Destroy(gameObject);
    }
}
