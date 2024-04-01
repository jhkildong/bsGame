using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon
{
    public class SpatialObject : MonoBehaviour, ISpatialData2D
    {
        Vector3 cachedPos;
        Rect? cachedBounds;
        Vector2? cached2dPos;
        float? cachedRadius;
        Collider myCollider;
        float width, height;
        MeshRenderer LinkedMeshRender;

        bool HasMoved
        {
            get
            {
                return !Mathf.Approximately((transform.position - cachedPos).sqrMagnitude, 0f);
            }
        }

        public Vector2 GetLocation()
        {
            if (cached2dPos == null)
            {
                CachePositionData();
            }
            return cached2dPos.Value;
        }

        public Rect GetBounds()
        {
            if (cachedBounds == null)
            {
                CachePositionData();
            }
            return cachedBounds.Value;
        }

        public float GetRadius()
        {
            if (cachedRadius == null)
            {
                CachePositionData();
            }
            return cachedRadius.Value;
        }
        
        void CachePositionData()
        {
            cachedPos = transform.position;
            cached2dPos = new Vector2(cachedPos.x, cachedPos.z);

            if (myCollider == null)
            {
                if (!TryGetComponent(out myCollider))
                {
                    myCollider = gameObject.AddComponent<CapsuleCollider>();
                    (myCollider as CapsuleCollider).radius = 0.5f;
                    (myCollider as CapsuleCollider).height = 2f;
                    (myCollider as CapsuleCollider).center = Vector3.up;  
                }
            }
            width = myCollider.bounds.size.x;
            height = myCollider.bounds.size.z;

            cachedBounds = new Rect(cached2dPos.Value.x - width, cached2dPos.Value.y - height, width, height);
            cachedRadius = Mathf.Sqrt(width * width + height * height);
        }

        void Start()
        {
            CachePositionData();

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // Sphere의 위치를 캐릭터의 머리 위로 설정
            Vector3 headPosition = this.transform.position;
            headPosition.y += 3.0f;
            sphere.transform.position = headPosition;
            sphere.transform.parent = this.transform;
            
            LinkedMeshRender = sphere.GetComponent<MeshRenderer>();
        }

        /*
        void Update()
        {

            if (cached2dPos == null || HasMoved)
            {
                cachedPos = transform.position;
                cached2dPos = new Vector2(cachedPos.x, cachedPos.z);
            }
        }
        */

        public void AddHighlight()
        {
           Debug.Log("AddHighlight");
            LinkedMeshRender.material.color = Color.red;
        }

        public void RemoveHighlight()
        {
            Debug.Log("RemoveHighlight");
            LinkedMeshRender.material.color = Color.white;
        }
    }
}

