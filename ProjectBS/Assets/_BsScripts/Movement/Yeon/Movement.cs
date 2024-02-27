using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon
{
    public class Movement : MonoBehaviour
    {
        protected Rigidbody rBody;

        [Range(1f, 10f), Tooltip("이동속도")]
        public float moveSpeed = 5f;

        public bool isMoving;
        public bool isBlocked; //장애물 존재
        public bool isOutOfControl; //제어 불가 상태

        public Vector3 worldMoveDir;
        public float outOfControllDuration;

        protected virtual void Start()
        {
            InitRigidbody();
        }

        private void InitRigidbody()
        {
            TryGetComponent(out rBody);
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
