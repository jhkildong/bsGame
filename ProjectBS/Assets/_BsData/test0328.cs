using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test0328 : MonoBehaviour
{
    void MoveToPos(Vector3 pos)
    {
        Vector3 Dir = pos - this.transform.position;
        float Dist = Dir.magnitude;
        Dir.Normalize();
    }
    IEnumerator Moving(Vector3 Dir, float Dist)
    {
        while (Dist > Mathf.Epsilon)
        {
            float delta = Time.deltaTime * 3.0f;
            if (Dist < delta)

            {
                delta = Dist;
            }
            Dist -= delta;
            this.transform.Translate(Dir * delta, Space.World);
            yield return null;
        }
    }
}
