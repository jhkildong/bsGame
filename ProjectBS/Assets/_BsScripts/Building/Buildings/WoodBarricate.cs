using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBarricate : MonoBehaviour
{
    [SerializeField]
    private BuildingScriptableObject buildingData;
    public BuildingScriptableObject BuildingData { set { buildingData = value; } }




    void Constructing()
    {
        
    }
    void Repairing()
    {

    }
    void GetDamaged()
    {

    }
    void Destroy()
    {
       if(buildingData.curHp < 0)
        {
            Destroy(gameObject);
        }
    }
}
