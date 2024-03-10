using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotion : MonoBehaviour
{
    Animator myAnim;
    Yeon.Movement myPlayer;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        myPlayer = GetComponentInParent<Yeon.Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        myPlayer.SetDirection(myAnim.velocity);
    }

    private void OnAnimatorMove()
    {

    }
}
