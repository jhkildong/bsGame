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
        protected Rigidbody rBody;

        [SerializeField, Range(1f, 10f), Tooltip("�̵��ӵ�")]
        protected float moveSpeed = 1f;

        [SerializeField] protected bool isMoving;
        [SerializeField] protected bool isBlocked = false; //��ֹ� ����
        [SerializeField] protected bool isOutOfControl = false; //���� �Ұ� ����

        [SerializeField] protected Vector3 worldMoveDir;
        [SerializeField] protected float outOfControllDuration;

        ///<summary>���۽� rigidBody�� ĸ���ݶ��̴� ����</summary>
        protected virtual void Start()
        {
            InitRigidbody();
            InitCapsuleCollider();
        }

        private void InitRigidbody()
        {
            if(TryGetComponent(out rBody) == false)
            {
                rBody = gameObject.AddComponent<Rigidbody>();
                rBody.useGravity = false;
                rBody.freezeRotation = true;
            }
        }

        private void InitCapsuleCollider()
        {
            CapsuleCollider capsule;
            TryGetComponent(out capsule);
            if (capsule == null)
            {
                capsule = gameObject.AddComponent<CapsuleCollider>();

                capsule.height = 2.0f;
                capsule.center = Vector3.up * 1.0f;
                capsule.radius = 0.5f;
            }
        }

        protected virtual void FixedUpdate()
        {
            MovementToRigidbody();
        }

        /// <summary> ������ٵ� ���� �ӵ� ���� </summary>
        private void MovementToRigidbody()
        {
            if (!isOutOfControl && !isBlocked)
            {
                rBody.velocity = worldMoveDir * moveSpeed;
            }
            else
            {
                rBody.velocity = new Vector3(0, 0, 0);
            }

        }

        public void SetDirection(Vector3 dir)
        {
            worldMoveDir = dir;
        }
    }

}
