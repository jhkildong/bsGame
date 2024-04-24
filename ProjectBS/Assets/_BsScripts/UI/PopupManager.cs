using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopupManager : MonoBehaviour
{
    private PopupWindow BlessPopup;
    private PopupWindow BuildingPopup;

    private bool blessSwitch;
    private bool buildingSwitch;
    // Start is called before the first frame update
    void Start()
    {
        BlessPopup = UIManager.Instance.CreateUI(UIID.BlessPopup, CanvasType.Canvas) as PopupWindow;
        BuildingPopup = UIManager.Instance.CreateUI(UIID.BuildingPopup, CanvasType.Canvas) as PopupWindow;

        blessSwitch = false;
        buildingSwitch = false;
        BlessPopup.gameObject.SetActive(blessSwitch);
        BuildingPopup.gameObject.SetActive(buildingSwitch);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            blessSwitch = !blessSwitch;
            BlessPopup.gameObject.SetActive(blessSwitch);
            if(blessSwitch) BlessPopup.transform.SetAsLastSibling();
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            buildingSwitch = !buildingSwitch;
            BuildingPopup.gameObject.SetActive(buildingSwitch);
            if(buildingSwitch) BuildingPopup.transform.SetAsLastSibling();
        }
    }
}
