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


        #region Component
        protected Rigidbody rBody;
        protected Collider col;
        #endregion

        #region Private Field
        [SerializeField]
        protected float moveSpeed = 1f;

        [SerializeField] protected bool isMoving;
        [SerializeField] protected bool isBlocked = false; //장애물 존재
        [SerializeField] protected bool isOutOfControl = false; //제어 불가 상태

        [SerializeField] protected Vector3 worldMoveDir;
        [SerializeField] protected float outOfControllDuration;
        #endregion

        #region Init Method
        private void InitRigidbody()
        {
            if (TryGetComponent(out rBody) == false)
            {
                rBody = gameObject.AddComponent<Rigidbody>();
                //rBody.useGravity = false;
                rBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                rBody.constraints |= RigidbodyConstraints.FreezePositionY;
            }
        }

        protected virtual void InitCollider()
        {
            TryGetComponent(out col);
            if (col == null)
            {
                col = gameObject.AddComponent<CapsuleCollider>();

                (col as CapsuleCollider).height = 2.0f;
                (col as CapsuleCollider).center = Vector3.up * 1.0f;
                (col as CapsuleCollider).radius = 0.5f;
            }
        }
        #endregion

        private void OnDrawGizmos()
        {
            if(col is CapsuleCollider cc)
            {

                Gizmos.color = Color.red;


                float height = cc.height * 0.5f; // 캡슐 콜라이더의 높이의 절반
                Vector3 center = transform.position + cc.center; // 캡슐 콜라이더의 중심 위치

                // 캡슐의 두 끝을 그립니다.
                Gizmos.DrawWireSphere(center + Vector3.up * (height - cc.radius), cc.radius);
                Gizmos.DrawWireSphere(center + Vector3.down * (height - cc.radius), cc.radius);

                // 캡슐의 두 끝을 연결합니다.
                Gizmos.DrawLine(center + Vector3.up * (height - cc.radius), center + Vector3.down * (height - cc.radius));
            }
        }

        #region Unity Event
        ///<summary>시작시 rigidBody와 캡슐콜라이더 설정</summary>
        protected virtual void Awake()
        {
            InitRigidbody();
            InitCollider();
        }

        protected virtual void FixedUpdate()
        {
            MovementToRigidbody();
        }
        #endregion

        #region Private Method
        /// <summary> 리지드바디 최종 속도 적용 </summary>
        private void MovementToRigidbody()
        {
            if (!isOutOfControl && !isBlocked && !isMoving)
            {
                rBody.velocity = worldMoveDir * moveSpeed;
            }
            else
            {
                rBody.velocity = new Vector3(0, 0, 0);
            }
        }
        #endregion

        #region Public Method
        public void SetDirection(Vector3 dir)
        {
            worldMoveDir = dir;
        }
        #endregion
    }

}
