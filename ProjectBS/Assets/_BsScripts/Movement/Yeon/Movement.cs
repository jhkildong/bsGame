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
        ////////////////////////////////Component////////////////////////////////
        protected Rigidbody rBody;
        protected Collider col;
        #endregion

        #region Private Field
        ////////////////////////////////Private Field////////////////////////////////
        
        [SerializeField]
        protected float moveSpeed = 1f;
        protected float moveSpeedBuff = 1f;

        protected bool isOutOfControl = false; //제어 불가 상태
        protected bool isStop = false; //이동 중지 상태
        protected Vector3 worldMoveDir;
        #endregion

        #region Init Method
        ////////////////////////////////InitMethod////////////////////////////////
        protected void InitRigidbody()
        {
            if (TryGetComponent(out rBody) == false)
            {
                rBody = gameObject.AddComponent<Rigidbody>();
                //rBody.useGravity = false;
                rBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                rBody.constraints |= RigidbodyConstraints.FreezePositionY;
            }
        }

        protected virtual void InitCollider(float radius = 0.5f)
        {
            TryGetComponent(out col);
            if (col == null)
            {
                col = gameObject.AddComponent<CapsuleCollider>();
                (col as CapsuleCollider).radius = radius;
                (col as CapsuleCollider).height = 2.0f;
            }
        }
        #endregion

        #region Unity Event
        ////////////////////////////////UnityEvent////////////////////////////////
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
        ////////////////////////////////PrivateMethod////////////////////////////////
        /// <summary> 리지드바디 최종 속도 적용 </summary>
        private void MovementToRigidbody()
        {
            if (!isOutOfControl && !isStop)
            {
                rBody.velocity = worldMoveDir * moveSpeed * moveSpeedBuff;
            }
            else
            {
                rBody.velocity = new Vector3(0, 0, 0);
            }
        }
        #endregion

        #region Public Method
        ////////////////////////////////Public Method////////////////////////////////
        public void SetDirection(Vector3 dir)
        {
            worldMoveDir = dir;
        }
        public void SetOutOfControl(bool outOfControl)
        {
            isOutOfControl = outOfControl;
        }
        #endregion
    }

}
