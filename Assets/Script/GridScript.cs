using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour {

    #region Gizmo Stats
    public bool drawGizmos = true;
    [Range(0, 1)]
    public float nodeThickness = .05f;
    [Range(0, 1)]
    public float nodeDiameter = .9f;
    public Color gridColor = Color.blue;
    public Color pathColor = Color.cyan;
    #endregion

    public int gridSizeX = 2;
    public int gridSizeY = 2;
    public int gridSizeZ = 2;

    public Transform movablePlatform;

    Node[,,] grid;

    public List<Node> path;

    public Orientation targetOrientation;


    public Slope[] slopes;
    public LayerMask slopeMask;

    public static Vector3[] OrientationVectors = new Vector3[]
{
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
};

    public Vector3Int inspectorPosition;
    private void Update() {
        var nodeInspector = true;

        if (nodeInspector) {
            var input = new Vector3Int();
            input.x += Input.GetKeyDown(KeyCode.A) ? 1 : 0;
            input.x -= Input.GetKeyDown(KeyCode.D) ? 1 : 0;

            input.y += Input.GetKeyDown(KeyCode.E) ? 1 : 0;
            input.y -= Input.GetKeyDown(KeyCode.Q) ? 1 : 0;

            input.z += Input.GetKeyDown(KeyCode.W) ? 1 : 0;
            input.z -= Input.GetKeyDown(KeyCode.S) ? 1 : 0;

            inspectorPosition += input;
        }
    }


    private void Awake() {
        CreateGrid();
    }

    public int MaxSize {
        get {
            return gridSizeX * gridSizeY * gridSizeZ;
        }
    }

    void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                for (int z = 0; z < gridSizeZ; z++) {
                    Vector3 worldPos = new Vector3(x, y, z) + Vector3.one * .5f;

                    grid[x, y, z] = new Node(worldPos, x, y, z);

                    for (int i = 0; i < OrientationVectors.Length; i++) {
                        Vector3 currentOrientation = OrientationVectors[i];
                        Vector3 reversedOrientation = currentOrientation * -1;
                        RaycastHit hit;
                        if (!Physics.CheckSphere(worldPos, .4f) && 
                            Physics.SphereCast(worldPos, .1f, currentOrientation, out hit, 1) && 
                            !Physics.Raycast(worldPos + reversedOrientation, currentOrientation, 1)) {

                            grid[x, y, z].walkable[i] = true;
                        }
                        else
                            grid[x, y, z].walkable[i] = false;
                    }
                }
            }
        }
    }
    public Node NodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = (worldPosition.x / gridSizeX);
        float percentY = (worldPosition.y / gridSizeY);
        float percentZ = (worldPosition.z / gridSizeZ);
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        return grid[x, y, z];
    }


    public List<Node> GetValidNeighbours(Node node, Orientation orientation) {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                for (int z = -1; z <= 1; z++) {
                    if (x == 0 && y == 0 && z == 0) //prevents neighbouring self.
                        continue;
                    Vector3 worldPos = node.worldPosition;
                    Vector3 localDown = Vector3.down;


                    // sets a local xyz axis in relation to the global xyz axis based on the seekers orientation. 
                    Vector3 currentNPos = node.worldPosition;

                    switch (targetOrientation) {
                        case Orientation.up:
                            currentNPos = node.worldPosition + new Vector3(x, -y, z);
                            localDown = Vector3.up;
                            break;
                        case Orientation.down:
                            currentNPos = node.worldPosition + new Vector3(x, y, z);
                            localDown = Vector3.down;
                            break;
                        case Orientation.left:
                            currentNPos = node.worldPosition + new Vector3(y, x, z);
                            localDown = Vector3.left;
                            break;
                        case Orientation.right:
                            currentNPos = node.worldPosition + new Vector3(-y, x, z);
                            localDown = Vector3.right;
                            break;
                        case Orientation.front:
                            currentNPos = node.worldPosition + new Vector3(x, z, -y);
                            localDown = Vector3.forward;
                            break;
                        case Orientation.back:
                            currentNPos = node.worldPosition + new Vector3(x, z, y);
                            localDown = Vector3.back;
                            break;
                        default:
                            break;
                    }



                    //localDown = NOrientation.OrientationVectors[(int)orientation];
                    Orientation searchDirection;
                    if (y == 0)
                        searchDirection = OrientationScript.Vector3ToOrientation(worldPos - currentNPos).Inverse();
                    else if (y > 0)
                        searchDirection = OrientationScript.Vector3ToOrientation(worldPos - currentNPos + -localDown).Inverse();
                    else
                        searchDirection = OrientationScript.Vector3ToOrientation(worldPos - currentNPos + localDown).Inverse();



                    //
                    //Cant walk from one slope to another unless new elevation
                    RaycastHit hitSlopeNeighbour;
                    if (Physics.Raycast(currentNPos, localDown, out hitSlopeNeighbour, 1, slopeMask) && y == 0) {
                        RaycastHit hitSlopeNode;
                        if (Physics.Raycast(worldPos, localDown, out hitSlopeNode, 1, slopeMask)) {
                            continue;
                        }
                        Slope slope = hitSlopeNeighbour.transform.GetComponent<Slope>();
                        print(slope.faceOrientation + "" + slope.downOrientation + "" + searchDirection.Inverse());
                        if (searchDirection.Inverse() == slope.faceOrientation || searchDirection.Inverse() == slope.downOrientation) {
                            print("found a slope! " + slope);

                        }
                        else
                            continue;
                    }
                    //
                    //if standing on slope, must exit front or back from it
                    RaycastHit hitSlopeNodeB;
                    if (Physics.Raycast(worldPos, localDown, out hitSlopeNodeB, 1, slopeMask) && y == 0) {
                        Slope slope = hitSlopeNodeB.transform.GetComponent<Slope>();
                        if (searchDirection == slope.faceOrientation || searchDirection == slope.downOrientation) {

                        }
                        else
                            continue;
                    }

                    //prevents diagonal movement.
                    bool shouldAbort = false;
                    for (int i = 0; i < OrientationVectors.Length; i++) {
                        if (Mathf.Abs(x) == Mathf.Abs(z))
                            shouldAbort = true;
                    }
                    if (shouldAbort)
                        continue;
                    //

                    // Used to make the player not able to climb
                    //NSlope slope = null;
                    if (y != 0) {
                        RaycastHit hit;
                        if (y < 0) {
                            if (Physics.Raycast(worldPos, localDown, out hit, 1, slopeMask)) {
                                Slope slopeA = hit.transform.GetComponent<Slope>();
                                if (slopeA.faceOrientation == searchDirection.Inverse() || slopeA.downOrientation == searchDirection.Inverse()) {
                                    if (slopeA.faceOrientation == orientation || slopeA.downOrientation == orientation) {
                                        if (Physics.Raycast(currentNPos, localDown, out hit, 1, slopeMask)) {
                                            //print(searchDirection);
                                            slopeA = hit.transform.GetComponent<Slope>();
                                            if (slopeA.faceOrientation == searchDirection.Inverse() || slopeA.downOrientation == searchDirection.Inverse()) {
                                                if (slopeA.faceOrientation == orientation || slopeA.downOrientation == orientation) {

                                                }
                                                else
                                                    continue;
                                            }
                                            else
                                                continue;
                                        }
                                    }
                                    else
                                        continue;
                                }
                                else
                                    continue;
                            }
                            else
                                continue;
                        }
                        else if (y > 0) {
                            if (Physics.Raycast(currentNPos, localDown, out hit, 1))
                                if (!hit.transform.GetComponent<Slope>())
                                    continue;
                                else {
                                    Slope slopeC = hit.transform.GetComponent<Slope>();
                                    if (slopeC.faceOrientation == orientation || slopeC.downOrientation == orientation) {
                                        if (searchDirection == slopeC.faceOrientation || searchDirection == slopeC.downOrientation) {

                                        }
                                        else {
                                            continue;
                                        }
                                    }
                                    else {
                                        continue;
                                    }
                                }
                        }

                    }

                    // Prevents climbing through the roof
                    if (y > 0) {
                        if (Physics.Linecast(worldPos, worldPos + -OrientationVectors[(int)targetOrientation]))
                            continue;
                    }
                    if (y < 0) {
                        if (Physics.Linecast(currentNPos, currentNPos - OrientationVectors[(int)targetOrientation]))
                            continue;
                    }
                    //

                    if (Mathf.Abs(x) == Mathf.Abs(z) /*&& y == 0*/) {//prevent crossing corners

                        if (Physics.Linecast(currentNPos, currentNPos + localDown)) {

                        }
                    }

                    // converts the local axis to global axis.
                    int checkX = 0, checkY = 0, checkZ = 0;
                    if (targetOrientation == Orientation.up) {
                        checkX = node.gridX + x;
                        checkY = node.gridY + -y;
                        checkZ = node.gridZ + z;
                    }
                    else if (targetOrientation == Orientation.down) {
                        checkX = node.gridX + x;
                        checkY = node.gridY + y;
                        checkZ = node.gridZ + z;
                    }
                    else if (targetOrientation == Orientation.left) {
                        checkX = node.gridX + y;
                        checkY = node.gridY + x;
                        checkZ = node.gridZ + z;
                    }
                    else if (targetOrientation == Orientation.right) {
                        checkX = node.gridX + -y;
                        checkY = node.gridY + x;
                        checkZ = node.gridZ + z;
                    }
                    else if (targetOrientation == Orientation.front) {
                        checkX = node.gridX + x;
                        checkY = node.gridY + z;
                        checkZ = node.gridZ + -y;
                    }
                    else if (targetOrientation == Orientation.back) {
                        checkX = node.gridX + x;
                        checkY = node.gridY + z;
                        checkZ = node.gridZ + y;
                    }

                    //checks if the nodes are out of bounds.
                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY && checkZ >= 0 && checkZ < gridSizeZ) {
                        neighbours.Add(grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }

        return neighbours;
    }

    public void MakeWalkable(Node node, bool _unwalkable) {
        if (!_unwalkable) {
            for (int i = 0; i < OrientationVectors.Length; i++) {
                NodeFromWorldPoint(node.worldPosition - OrientationVectors[i]).walkable[i] = false;
            }
            return;
        }
        for (int i = 0; i < OrientationVectors.Length; i++) {


            if (!Physics.CheckSphere(node.worldPosition, .4f) && Physics.Linecast(node.worldPosition, node.worldPosition + OrientationVectors[i]) && !Physics.Raycast(node.worldPosition + -OrientationVectors[i], OrientationVectors[i], 1))
                node.walkable[i] = true;
            else
                node.walkable[i] = false;

            for (int j = 0; j < OrientationVectors.Length; j++) {
                Vector3 neighbourPos = node.worldPosition + -OrientationVectors[j];
                if (!Physics.CheckSphere(neighbourPos, .4f) && Physics.Linecast(neighbourPos, neighbourPos + OrientationVectors[j]) && !Physics.Raycast(neighbourPos + -OrientationVectors[j], OrientationVectors[j], 1))
                    NodeFromWorldPoint(neighbourPos).walkable[j] = true;
                else
                    NodeFromWorldPoint(neighbourPos).walkable[j] = false;

            }

        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(gridSizeX * .5f, gridSizeY * .5f, gridSizeZ * .5f), new Vector3(gridSizeX, gridSizeY, gridSizeZ)); 
        if (drawGizmos != true) {
            return;
        }

        if (grid != null) {
            foreach (var n in grid) {
                Gizmos.color = gridColor;

                for (int i = 0; i < OrientationVectors.Length; i++) {
                    Vector3 _ov = OrientationVectors[i];
                    if (n.walkable[i]) {
                        Gizmos.DrawWireCube(
                            n.worldPosition + _ov * .5f - _ov * nodeThickness * .5f,
                            _ov.Abs() * nodeThickness + (Vector3.one - _ov.Abs()) * nodeDiameter);
                    }
                }
                if (n.currentObject)
                Gizmos.DrawSphere(n.worldPosition, .5f);
                Gizmos.color = Color.white;
            }

            if (path != null) {
                foreach (var node in path) {
                    Gizmos.color = pathColor;
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * .2f);
                }
            }
        }
    }
}
