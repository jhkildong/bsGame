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
        protected Rigidbody rBody;
        protected Collider col;
        #endregion

        #region Private Field
        [SerializeField]
        protected float moveSpeed = 1f;

        [SerializeField] protected bool isMoving;
        [SerializeField] protected bool isBlocked = false; //��ֹ� ����
        [SerializeField] protected bool isOutOfControl = false; //���� �Ұ� ����

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


                float height = cc.height * 0.5f; // ĸ�� �ݶ��̴��� ������ ����
                Vector3 center = transform.position + cc.center; // ĸ�� �ݶ��̴��� �߽� ��ġ

                // ĸ���� �� ���� �׸��ϴ�.
                Gizmos.DrawWireSphere(center + Vector3.up * (height - cc.radius), cc.radius);
                Gizmos.DrawWireSphere(center + Vector3.down * (height - cc.radius), cc.radius);

                // ĸ���� �� ���� �����մϴ�.
                Gizmos.DrawLine(center + Vector3.up * (height - cc.radius), center + Vector3.down * (height - cc.radius));
            }
        }

        #region Unity Event
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
        /// <summary> ������ٵ� ���� �ӵ� ���� </summary>
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
