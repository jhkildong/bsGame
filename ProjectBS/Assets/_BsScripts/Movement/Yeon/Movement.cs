using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon
{
    /*
    �̵� ���� ������ Yeon.Movement class ���
    �̵��� �Ϸ��� �ϴ� ���� ���Ͱ��� FixedUpdate���� worldMoveDir�� ����
    rigidbody�� velocity������ ���� �̵�
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

        protected bool isOutOfControl = false; //���� �Ұ� ����
        protected bool isStop = false; //�̵� ���� ����
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
        ///<summary>���۽� rigidBody�� ĸ���ݶ��̴� ����</summary>
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
        /// <summary> ������ٵ� ���� �ӵ� ���� </summary>
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
