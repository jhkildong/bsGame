using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPoint : MonoBehaviour
{
    public LayerMask mask;
    private Camera myCam;
    private Animator myAnim;
    // Start is called before the first frame update
    void Start()
    {
        myCam = Camera.main;
        myAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (myAnim.GetBool(AnimParam.isAttacking))
        //    return;
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, mask))
        {
            if ((mask & 1 << hit.transform.gameObject.layer) != 0)
            {
                Vector3 targetDirection = hit.point - transform.position;
                targetDirection.y = 0f; // y축 회전을 방지하기 위해 0으로 설정
                if (targetDirection.sqrMagnitude >= 0.5f)
                {
                    targetDirection.Normalize();
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

                }
            }
        }
    }
}
