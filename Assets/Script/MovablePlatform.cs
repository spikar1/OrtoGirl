using UnityEngine;
using System.Collections;

[AddComponentMenu("Interactables/ Movable Platform")]
public class MovablePlatform : InteractableBlock {

    public Transform endTransform;

    public Orientation startOrientation;
    public Orientation endOrientation;

    public Vector3 forward;
    Vector3 startPosition;
    Quaternion startRotation;

    public bool atStart = true;

	// Use this for initialization
	void Awake () {
        if (endTransform == null)
            endTransform = transform;
        startPosition = transform.position;
        startRotation = transform.rotation;
        
	}
	
	// Update is called once per frame
	public override void Update () {
        if (Input.GetKeyDown(KeyCode.M)) {
            StartMove();
        }
        forward = transform.forward;
    }
    public override void Start() {
        base.Start();
    }
    public override void Activate()
    {
        base.Activate();
        StartMove();
    }

    public void StartMove() {
        if (atStart)
            StartCoroutine(MoveTowards(endTransform.position, endTransform.rotation));
        else
            StartCoroutine(MoveTowards(startPosition, startRotation));
    }

    IEnumerator MoveTowards ( Vector3 endPos, Quaternion endRot)
    {
        Vector3 posToMakeUnwalkable = transform.position;
        
        while (transform.position - endPos != Vector3.zero || transform.rotation != endRot ) {
            transform.position = Vector3.MoveTowards(transform.position, endPos, .1f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, endRot, 2);
            if (Vector3.Magnitude(transform.position - endPos) < 0.1 && endRot == transform.rotation)
            {
                transform.position = endPos;
                transform.rotation = endRot;
            }
            yield return null;
        }
        atStart = !atStart;
        grid.MakeWalkable(grid.NodeFromWorldPoint(posToMakeUnwalkable), false);
        grid.MakeWalkable(grid.NodeFromWorldPoint(transform.position), true);

        grid.NodeFromWorldPoint(posToMakeUnwalkable).currentObject = null;
        grid.NodeFromWorldPoint(transform.position).currentObject = this.gameObject;

        if (atStart)
            grid.targetOrientation = startOrientation;
        else
            grid.targetOrientation = endOrientation;
    }

    
}
