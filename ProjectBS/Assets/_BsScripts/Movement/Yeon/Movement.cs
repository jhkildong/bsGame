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
        protected float moveSpeed = 5f;

        [SerializeField] protected bool isMoving;
        [SerializeField] protected bool isBlocked; //��ֹ� ����
        [SerializeField] protected bool isOutOfControl; //���� �Ұ� ����

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

        /// <summary> ������ٵ� ���� �ӵ� ���� </summary>
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
