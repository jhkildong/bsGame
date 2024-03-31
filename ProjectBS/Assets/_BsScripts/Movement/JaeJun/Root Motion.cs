using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotion : MonoBehaviour
{
    [SerializeField]Animator myAnim;
    Yeon.Movement myPlayer
    {
        get
        {
            if (_myPlayer == null)
                _myPlayer = GetComponentInParent<Yeon.Movement>();
            return _myPlayer;
        }
    }

    [SerializeField]Yeon.Movement _myPlayer;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(myAnim == null)
            myAnim = GetComponent<Animator>();
        if(_myPlayer == null)
            _myPlayer = GetComponentInParent<Yeon.Movement>();
    }

    private void OnAnimatorMove()
    {
        myPlayer.transform.position = myAnim.rootPosition;
    }
}
