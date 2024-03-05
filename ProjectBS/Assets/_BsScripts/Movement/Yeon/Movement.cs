using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon
{
    /*
    이동 조작 구현시 Yeon.Movement class 상속
    이동을 하려고 하는 방향 벡터값을 FixedUpdate에서 worldMoveDir에 전달
    rigidbody의 velocity조작을 통한 이동
    */
    public class Movement : MonoBehaviour
    {
        protected Rigidbody rBody;

        [SerializeField, Range(1f, 10f), Tooltip("이동속도")]
        protected float moveSpeed = 5f;

        [SerializeField] protected bool isMoving;
        [SerializeField] protected bool isBlocked; //장애물 존재
        [SerializeField] protected bool isOutOfControl; //제어 불가 상태

        [SerializeField] protected Vector3 worldMoveDir;
        [SerializeField] protected float outOfControllDuration;

        protected virtual void Start()
        {
            InitRigidbody();
        }

        private void InitRigidbody()
        {
            if(TryGetComponent(out rBody) == false)
            {
                rBody = gameObject.AddComponent<Rigidbody>();
            }
        }

        protected virtual void FixedUpdate()
        {
            MovementToRigidbody();
        }

        /// <summary> 리지드바디 최종 속도 적용 </summary>
        private void MovementToRigidbody()
        {
            if (!isOutOfControl)
            {
                rBody.velocity = worldMoveDir * moveSpeed;
            }
            else
            {
                rBody.velocity = new Vector3(0, 0, 0);
            }

        }
    }

}
