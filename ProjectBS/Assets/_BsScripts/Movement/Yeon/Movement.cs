using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon
{
    public class Movement : MonoBehaviour
    {
        protected Rigidbody rBody;

        [Range(1f, 10f), Tooltip("�̵��ӵ�")]
        public float moveSpeed = 5f;

        public bool isMoving;
        public bool isBlocked; //��ֹ� ����
        public bool isOutOfControl; //���� �Ұ� ����

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
