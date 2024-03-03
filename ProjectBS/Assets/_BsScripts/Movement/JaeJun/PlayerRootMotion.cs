using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRootMotion : MonoBehaviour
{
    Animator myAnim;
    // Start is called before the first frame update
    protected void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    private void OnAnimatorMove()
    {
        transform.parent.position += myAnim.deltaPosition;
        transform.parent.rotation *= myAnim.deltaRotation;
    }
}

