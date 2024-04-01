using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class Player2D : MonoBehaviour
{
    [SerializeField] QuadTree LinkedQuadTree;
    [SerializeField] float ObstacleSearchRange = 30f;

    [SerializeField] Vector3 CachedPosition;
    Vector2? Cached2DPosition => _Cached2DPosition;

    [SerializeField] Vector2 _Cached2DPosition;

    HashSet<ISpatialData2D> NearbyObstacles;

    bool HasMoved
    {
        get
        {
            return !Mathf.Approximately((transform.position - CachedPosition).sqrMagnitude, 0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Cached2DPosition == null || HasMoved)
        {
            CachedPosition = transform.position;
            _Cached2DPosition = new Vector2(CachedPosition.x, CachedPosition.z);

            HighlightNearbyObstacles();
        }
    }

    void HighlightNearbyObstacles()
    {
        HashSet<ISpatialData2D> CandidateObstacles = LinkedQuadTree.FindDataInRange(Cached2DPosition.Value, ObstacleSearchRange);

        // identify removals
        if (NearbyObstacles != null)
        {
            foreach (var OldObstacle in NearbyObstacles)
            {
                if (CandidateObstacles.Contains(OldObstacle))
                    continue;

                ProcessRemoveObstacle(OldObstacle);
            }
        }

        if (CandidateObstacles == null)
        {
            Debug.Log("No obstacles found");
            return;
        }
            

        // first time finding obstacles?
        if (NearbyObstacles == null)
        {
            Debug.Log("First time finding obstacles");
            NearbyObstacles = CandidateObstacles;
            foreach (var NewObstacle in NearbyObstacles)
                ProcessAddObstacle(NewObstacle);

            return;
        }

        // identify additions
        foreach (var NewObstacle in CandidateObstacles)
        {
            if (NearbyObstacles.Contains(NewObstacle))
                continue;

            ProcessAddObstacle(NewObstacle);
        }

        NearbyObstacles = CandidateObstacles;
    }

    void ProcessAddObstacle(ISpatialData2D AddedObstacle)
    {
        (AddedObstacle as SpatialObject).AddHighlight();
    }

    void ProcessRemoveObstacle(ISpatialData2D RemovedObstacle)
    {
        (RemovedObstacle as SpatialObject).RemoveHighlight();
    }
}
