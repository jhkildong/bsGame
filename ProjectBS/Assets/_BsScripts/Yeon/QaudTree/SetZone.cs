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
        foreach (var mesh in meshes)
        {
            width += mesh.bounds.size.x;
            height += mesh.bounds.size.z;
        }

        var SpawnRect = new Rect(meshes[0].bounds.min.x, meshes[0].bounds.min.z, width, height);
        Debug.Log(SpawnRect);
        quadTree.On2DBoundsCalculated(SpawnRect);
    }
}
