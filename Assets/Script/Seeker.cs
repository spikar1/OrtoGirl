using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour {
    
    public Orientation orientation;

    public Orientation jumpDirection;

    public LayerMask gridLayer;

    public GridScript grid;
    //NPathfinding pathfinder;

    public Node nodeStandingOn;

    Player player;


    // [HideInInspector]
    public bool isWalking;

    int steps = 0;
    int nextStep;

    void Awake() {
        if (!grid) {
            if (GameObject.Find("AStar")) {
                grid = GameObject.Find("AStar").GetComponent<GridScript>();
            }
        }
        orientation = grid.targetOrientation;
        player = GetComponent<Player>();
    }

    void Update() {
        orientation = grid.targetOrientation;
    }

    public void Jump(Orientation _orientation) {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + _orientation.ToVector(), orientation.ToVector(), out hit, 100, gridLayer) &&
            !Physics.Raycast(transform.position, _orientation.ToVector(), 1)) {
            transform.position += OrientationScript.OrientationVectors[(int)_orientation];
            StartCoroutine(moveTowardsPoint(grid.NodeFromWorldPoint(hit.point + hit.normal * .8f).worldPosition));
        }

    }

    public IEnumerator moveTowardsPoint(Vector3 position) {


        while (transform.position != position) {
            print("Is falling");
            transform.position = Vector3.MoveTowards(transform.position, position, 1f * Time.deltaTime);
            yield return null;
        }
        ActivateBlock();

    }

    public void MoveTowardsTarget() {
        if (grid.path != null) {
            isWalking = true;
            foreach (var node in grid.path) {
                steps++;
            }
            InvokeRepeating("MoveToNode", 0, .0001f);
            StartCoroutine(MoveToNode());
        }



    }
    public IEnumerator MoveToNode() {
        while (nextStep < steps) {
            transform.position = Vector3.MoveTowards(transform.position, grid.path[nextStep].worldPosition, 5f * Time.deltaTime);
            if (transform.position == grid.path[nextStep].worldPosition) {
                nextStep++;
            }
            yield return null;
        }
        CancelWalk();
        player.OnStop();
        ActivateBlock();
    }

    public void ActivateBlock() {
        RaycastHit hit;
        Physics.Raycast(transform.position, orientation.ToVector(), out hit, 1f, gridLayer);
        Debug.DrawRay(transform.position, orientation.ToVector(), Color.red, 10);
        transform.parent = hit.transform;
        nodeStandingOn = grid.NodeFromWorldPoint(transform.position + grid.targetOrientation.ToVector());

        if (nodeStandingOn.currentObject != null) {
            transform.parent = nodeStandingOn.currentObject.transform;
            InteractableBlock[] blocksToActivate = nodeStandingOn.currentObject.GetComponents<InteractableBlock>();
            foreach (var block in blocksToActivate) {
                block.Activate();
            }
            //nodeStandingOn.currentObject.GetComponent<NInteractableBlocks>().Activate();
            
        }
    }

    public void CancelWalk() {
        isWalking = false;
        steps = 0;
        nextStep = 0;
        CancelInvoke();
    }
}
