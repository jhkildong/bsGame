using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class SetZone : MonoBehaviour
{
    public QuadTree quadTree;
    MeshRenderer[] meshes;
    float width = 0.0f, height = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        meshes = GetComponentsInChildren<MeshRenderer>();
        width += meshes[0].bounds.size.x;
        height += meshes[0].bounds.size.z;
        

        var SpawnRect = new Rect(meshes[0].bounds.min.x, meshes[0].bounds.min.z, width * 4, height * 4);
        Debug.Log(SpawnRect);
        quadTree.On2DBoundsCalculated(SpawnRect);
    }
}
