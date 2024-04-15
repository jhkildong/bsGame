using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BowAnimSet : MonoBehaviour
{
    [SerializeField]private Animator myAnim;

    #region MultiParent
    ////////////////////////////////MultiParent////////////////////////////////
    [SerializeField]private GameObject ArrowMultiParent;
    [SerializeField]private GameObject DummyArrow;
    private enum Mode
    {
        Idle,
        ArrowFinger,
        ArrowString,
        ArrowShot
    }
    private Mode mode = Mode.Idle;

    private void Start()
    {
        DummyArrow.SetActive(false);
    }
    //업데이트에서 SetWeight를 안하면 동작을 안함 Unity Animation rigging 예제 참고
    private void Update()
    {
        if(mode != Mode.Idle)
        {
            var ArrowConstraint = ArrowMultiParent.GetComponent<MultiParentConstraint>();
            var sourceObjects = ArrowConstraint.data.sourceObjects;

            sourceObjects.SetWeight(0, mode == Mode.ArrowFinger ? 1f : 0f);
            sourceObjects.SetWeight(1, mode == Mode.ArrowString ? 1f : 0f);
            ArrowConstraint.data.sourceObjects = sourceObjects;

            mode = Mode.Idle;
        }
    }

    public void SetArrowFinger()
    {
        DummyArrow.SetActive(true);
        mode = Mode.ArrowFinger;
    }
    public void SetArrowString()
    {
        mode = Mode.ArrowString;
    }
    public void SetArrowShot()
    {
        DummyArrow.SetActive(false);
        mode = Mode.ArrowShot;
    }
    #endregion

    #region IK
    ////////////////////////////////IK////////////////////////////////
    [SerializeField]private Transform bowString;

    bool ikActive = false;
    float weight = 0.0f;
    private void OnAnimatorIK(int layerIndex)
    {
        myAnim.SetIKPosition(AvatarIKGoal.RightHand, bowString.position);
        if(ikActive)
        {
            if(weight < 1.0f)
            {
                weight += Time.deltaTime;
            }
            else
            {
                weight = 1.0f;
            }
            myAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
        }
        else
        {
            weight = 0.0f;
            myAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.0f);
        }
    }

    public void SetIK()
    {
        ikActive = true;
    }

    public void ClearIK()
    {
        ikActive = false;
    }
    #endregion
}
