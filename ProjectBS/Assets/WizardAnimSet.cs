using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimSet : MonoBehaviour
{
    [SerializeField] private Animator myAnim;
    [SerializeField] private Transform effectSpawn;

    bool ikActive = false;
    float weight = 0.0f;
    private void Start()
    {
        if(myAnim == null)
        {
            myAnim = GetComponent<Animator>();
        } 
    }

    private void OnAnimatorIK(int layerIndex)
    {
        myAnim.SetIKPosition(AvatarIKGoal.RightHand, effectSpawn.position);
        if (ikActive)
        {
            if (weight < 1.0f)
            {
                weight += Time.deltaTime * 10;
                weight = Mathf.Min(weight, 1.0f);
            }
            myAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
        }
        else
        {
            if (weight > 0.0f)
            {
                weight -= Time.deltaTime * 10;
                weight = Mathf.Max(weight, 0.0f);
            }
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
}
