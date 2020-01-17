using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public Transform targetPos, player;
    public Target target;
    public LayerMask walkableLayers;


    GridScript grid;
    Vector3 lastTargetPosition;

    public bool validPath;

    void Awake() {
        grid = GetComponent<GridScript>();

        if (!target)
            target = GameObject.Find("Target").GetComponent<Target>();
        if (!targetPos)
            targetPos = GameObject.Find("Target").transform;
        if (!player)
            player = GameObject.Find("Player").transform;
    }

    void Update() {
        //FindPath(seeker.position, target.position);
        if (Input.GetKeyDown(KeyCode.Space))
            FindPath(player.position, targetPos.position, grid.targetOrientation, target.orientation);


        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000, walkableLayers)) {
                Debug.DrawLine(hit.point, hit.point + hit.normal * .4f, Color.yellow, 1);
                //targetPos.position = grid.NodeFromWorldPoint(grid.NodeFromWorldPoint(hit.point - hit.normal / 2).worldPosition +hit.normal).worldPosition;
                targetPos.position = grid.NodeFromWorldPoint(hit.point + hit.normal * .5f).worldPosition;
                target.orientation = hit.normal.Vector3ToOrientation().Inverse();
                if (validPath && targetPos.position == lastTargetPosition) {
                    player.GetComponent<Seeker>().MoveTowardsTarget();
                    validPath = false;
                }

                else {
                    //print(hit.collider.gameObject.name);
                    player.GetComponent<Seeker>().CancelWalk();
                    //grid.CreateGrid();
                    validPath = FindPath(player.position, targetPos.position, grid.targetOrientation, target.orientation);
                }
                lastTargetPosition = targetPos.position;
            }
        }
    }

    public bool FindPath(Vector3 startPos, Vector3 targetPos, Orientation seekerOrientation, Orientation targetOrientation, bool additive = false) {

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);



        if (targetOrientation != seekerOrientation) {
            for (int i = 0; i < grid.slopes.Length; i++) {
                Slope currentSlope = grid.slopes[i];

                if (currentSlope.downOrientation == targetOrientation && currentSlope.faceOrientation == seekerOrientation ||
                    currentSlope.downOrientation == seekerOrientation && currentSlope.faceOrientation == targetOrientation) {
                    if (FindPath(startPos, currentSlope.transform.position + grid.targetOrientation.Inverse().ToVector(), grid.targetOrientation, grid.targetOrientation)) {
                        print("found path!");
                    }
                    else
                        print("did not find path...");
                }
            }
            return false;
        }

        while (openSet.Count > 0) {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                //print("path found");
                RetracePath(startNode, targetNode);
                return true;
            }

            foreach (Node neighbour in grid.GetValidNeighbours(currentNode, grid.targetOrientation)) {

                if (closedSet.Contains(neighbour)) {
                    continue;
                }

                if (!neighbour.walkable[(int)grid.targetOrientation])
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    else
                        openSet.UpdateItem(neighbour);
                }
            }

        }
        return false;

    }

    void RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    /* private delegate int DistanceDelegate(int dstX, int dstY, int dstZ);

     private static int UpDistance(int dstX, int dstY, int dstZ)
     {
         if (dstX > dstZ)
             if (dstY > 0)
                 return dstZ * 14 + (dstX - dstZ) * 10 + dstY * 6;
             else if (dstY < 0)
                 return dstZ * 14 + (dstX - dstZ) * 10 + dstY * 14;
         if (dstX < dstZ)
             if (dstY > 0)
                 return dstX * 14 + (dstX - dstZ) * -10 + dstY * 6;
             else if (dstY < 0)
                 return dstX * 14 + (dstX - dstZ) * -10 + dstY * 14;
         return -1;
     }


     private DistanceDelegate[] distanceDelegates = new DistanceDelegate[] { UpDistance };*/



    int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);




        int localX, localY, localZ;
        localX = localY = localZ = 0;


        switch (grid.targetOrientation) {
            case Orientation.up:
            case Orientation.down:
                localX = dstX;
                localY = dstY;
                localZ = dstZ;
                break;
            case Orientation.left:
            case Orientation.right:
                localX = dstY;
                localY = dstX;
                localZ = dstZ;
                break;
            case Orientation.front:
            case Orientation.back:
                localX = dstX;
                localY = dstZ;
                localZ = dstY;
                break;
            default:
                break;
        }

        if (localX > localZ)
            if (localY > 0)
                return localZ * 14 + (localX - localZ) * 10 + localY * (localY > 0 ? 14 : 6); // 14 : 6

        if (localX < localZ)
            if (localY > 0)
                return localX * 14 + (localZ - localX) * 10 + localY * (localY > 0 ? 14 : 6); // 14 : 6

        return dstX * 10 + dstY * 10 + dstZ * 10;
    }
}
