#define QUADTREE_TrackStats

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon
{
    /// <summary>
    /// reference https://github.com/GameDevEducation/UT_DataStructures/tree/Part-1-Quadtrees
    /// </summary>


    public interface ISpatialData2D
    {
        Vector2 GetLocation();
        Rect GetBounds();
        float GetRadius();
    }

    public class QuadTree : MonoBehaviour
    {
        /// <summary>
        /// 쿼드트리 노드
        /// </summary>
        class Node
        {
            Rect Bounds;
            Node[] Children;
            int Depth = -1;

            HashSet<ISpatialData2D> Data;

            public Node(Rect bounds, int depth = 0)
            {
                Bounds = bounds;
                Depth = depth;
            }

            public void AddData(QuadTree Owner, ISpatialData2D data)
            {
                if (Children == null)
                {
                    //null일 경우 초기화
                    if (Data == null)
                        Data = new();

                    //최대데이터만큼 저장했고 분리가 가능한 경우
                    if (((Data.Count + 1) >= Owner.PreferredMaxDataPerNode) && CanSplit(Owner))
                    {
                        SplitNode(Owner);

                        AddDataToChildren(Owner, data);
                    }
                    else
                        Data.Add(data);

                    return;
                }

                AddDataToChildren(Owner, data);
            }

            bool CanSplit(QuadTree Owner)
            {
                return (Bounds.width >= (Owner.MinimumNodeSize * 2)) &&
                    (Bounds.height >= (Owner.MinimumNodeSize * 2));
            }

            void SplitNode(QuadTree Owner)
            {
                float halfWidth = Bounds.width * 0.5f;
                float halfHeight = Bounds.height * 0.5f;
                int newDepth = Depth + 1;
                float xMin = Bounds.xMin, yMin = Bounds.yMin;

#if QUADTREE_TrackStats
                Owner.NewNodesCreated(4, newDepth);
#endif //QUADTREE_TrackStats

                Children = new Node[4]
                {
                    new Node(new Rect(xMin, yMin, halfWidth, halfHeight), newDepth), //Bottom Left
                    new Node(new Rect(xMin + halfWidth, yMin, halfWidth, halfHeight), newDepth), //Bottom Right
                    new Node(new Rect(xMin, yMin + halfHeight, halfWidth, halfHeight), newDepth), //Top Left
                    new Node(new Rect(xMin + halfWidth, yMin + halfHeight, halfWidth, halfHeight), newDepth) //Top Right
                };

                //데이터를 자식노드에게 분배
                foreach(var data in Data)
                {
                    AddDataToChildren(Owner, data);
                }

                Data = null;
            }

            void AddDataToChildren(QuadTree Owner, ISpatialData2D data)
            {
                foreach (var child in Children)
                {
                    if (child.Overlaps(data.GetBounds()))
                        child.AddData(Owner, data);
                }
            }

            bool Overlaps(Rect bounds)
            {
                return Bounds.Overlaps(bounds);
            }

            public void FindDataInBox(Rect SearchRect, HashSet<ISpatialData2D> OutFoundData)
            {
                if (Children == null)
                {
                    if (Data == null || Data.Count == 0)
                        return;

                    OutFoundData.UnionWith(Data);

                    return;
                }

                foreach (var Child in Children)
                {
                    if (Child.Overlaps(SearchRect))
                        Child.FindDataInBox(SearchRect, OutFoundData);
                }
            }

            public void FindDataInRange(Vector2 SearchLocation, float SearchRange, HashSet<ISpatialData2D> OutFoundData)
            {
                if (Depth != 0)
                {
                    throw new System.InvalidOperationException("FindDataInRange cannot be run on anything other than the root node.");
                }

                Rect SearchRect = new Rect(SearchLocation.x - SearchRange, SearchLocation.y - SearchRange,
                                           SearchRange * 2f, SearchRange * 2f);

                FindDataInBox(SearchRect, OutFoundData);

                OutFoundData.RemoveWhere(data => {
                    float TestRange = SearchRange + data.GetRadius();

                    return (SearchLocation - data.GetLocation()).sqrMagnitude > (TestRange * TestRange);
                });
            }
        }
        
        [field: SerializeField] public int PreferredMaxDataPerNode { get; private set; } = 50;
        [field: SerializeField] public int MinimumNodeSize { get; private set; } = 2;

        Node RootNode;

        public void PrepareTree(Rect bounds)
        {
            RootNode = new Node(bounds);

#if QUADTREE_TrackStats
            NumNodes = 0;
            MaxDepth = -1;
#endif //QUADTREE_TrackStats
        }

        public void AddData(ISpatialData2D data)
        {
            RootNode.AddData(this, data);
        }

        public void ShowStats()
        {
#if QUADTREE_TrackStats
            Debug.Log($"Max Depth: {MaxDepth}, Num Nodes: {NumNodes}");
#endif //QUADTREE_TrackStats
        }

        public HashSet<ISpatialData2D> FindDataInRange(Vector2 SearchLocation, float SearchRange)
        {

            HashSet<ISpatialData2D> FoundData = new();
            RootNode.FindDataInRange(SearchLocation, SearchRange, FoundData);

            return FoundData;
        }

        public void On2DBoundsCalculated(Rect Bounds)
        {
            PrepareTree(Bounds);
        }

        public void OnItemSpawned(GameObject ItemGO)
        {
            AddData(ItemGO.GetComponent<ISpatialData2D>());
        }

        public void OnAllItemsSpawned(List<GameObject> Items)
        {
            // Intentionally turned off as only one of the add methods should be used

            //List<ISpatialData2D> SpatialItems = new List<ISpatialData2D>(Items.Count);
            //foreach (GameObject Item in Items)
            //{
            //    SpatialItems.Add(Item.GetComponent<ISpatialData2D>());
            //}

            //LinkedQuadTree.AddData(SpatialItems);

            ShowStats();
        }


#if QUADTREE_TrackStats
        int MaxDepth = -1;
        int NumNodes = 0;

        public void NewNodesCreated(int NumAdded, int NodeDepth)
        {
            NumNodes += NumAdded;
            MaxDepth = Mathf.Max(MaxDepth, NodeDepth);
            Debug.Log($"New Nodes Created: {NumAdded}, Depth: {NodeDepth}");
        }
#endif //QUATREE_TrackStats
    }
}

