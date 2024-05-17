using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotion : MonoBehaviour
{
    public bool isStop = false;
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
        if (isStop)
        {
            myAnim.rootPosition = transform.position;
            return;
        }
        if(myPlayer == null)
            transform.position = myAnim.rootPosition;
        else
            myPlayer.transform.position = myAnim.rootPosition;
    }
}
