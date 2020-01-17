using UnityEngine;
using UnityEditor;
using System.Collections;

public class Elevator : InteractableBlock {
    [HideInInspector]
    public bool useFreeTransform;
    [HideInInspector]
    public bool oneTimeUse;

    [HideInInspector]
    public Vector3 endPosHandle;
    [HideInInspector]
    public Vector3 endPos;



    public Orientation direction;
    [Range(1,50)]
    public int distance = 1;


    private Vector3 startPos;
    private bool isAtStart = true;
    //private bool isMoving = false;
    private bool deactivate = false;

    // Use this for initialization
    public override void Start () {
        startPos = transform.position;
        base.Start();
        
    }
	
	// Update is called once per frame
	public  override void Update ()
    {
        base.Update();
        /*if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
            ActivateElevator();*/
	}

    public override void Activate()
    {
        if (deactivate)
            return;
        
        base.Activate();
        ActivateElevator();
        if (oneTimeUse)
            deactivate = true;
    }

    public void ActivateElevator() {
        if (!useFreeTransform)
            if (isAtStart)
                StartCoroutine(MoveTowards(transform.position + GridScript.OrientationVectors[(int)direction] * distance));
            else
                StartCoroutine(MoveTowards(startPos));
        else
            if (isAtStart)
                StartCoroutine(MoveTowards(endPos));
            else
                StartCoroutine(MoveTowards(startPos));
        
    }

    IEnumerator MoveTowards(Vector3 targetPos) {
        Vector3 startPos = transform.position;
        //isMoving = true;
        while (transform.position != targetPos) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, .08f);
            yield return null;
        }
        grid.MakeWalkable(grid.NodeFromWorldPoint(startPos),    true);
        grid.MakeWalkable(grid.NodeFromWorldPoint(targetPos),   true);
        isAtStart = !isAtStart;

        grid.NodeFromWorldPoint(startPos).currentObject = null;
        grid.NodeFromWorldPoint(transform.position).currentObject = this.gameObject;

        //isMoving = false;

    }

    public void MoveToStart() {
        isAtStart = false;
        ActivateElevator();
    }

    public void MoveToEnd() {
        isAtStart = true;
        ActivateElevator();
    }

    public void FlipStartPosition() {
        if (useFreeTransform)
        {
            transform.position = endPos;
            endPos = startPos;
            startPos = transform.position;
            return;
        }
        transform.position = startPos = transform.position + GridScript.OrientationVectors[(int)direction] * distance;
        switch (direction)
        {
            case Orientation.up:
                direction = Orientation.down;
                break;
            case Orientation.down:
                direction = Orientation.up;
                break;
            case Orientation.left:
                direction = Orientation.right;
                break;
            case Orientation.right:
                direction = Orientation.left;
                break;
            case Orientation.front:
                direction = Orientation.back;
                break;
            case Orientation.back:
                direction = Orientation.front;
                break;
            default:
                direction = Orientation.down;
                break;
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            startPos = transform.position;

        if (!useFreeTransform)
        {
            Vector3 directionVector = GridScript.OrientationVectors[(int)direction];

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(startPos, startPos + directionVector * distance);
            Gizmos.DrawWireCube(startPos, GetComponent<Collider>().bounds.size);
            Gizmos.DrawWireCube(startPos + directionVector * distance, GetComponent<Collider>().bounds.size);
            Gizmos.DrawCube(startPos + directionVector * distance, Vector3.one * .2f);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(startPos, endPos);
            Gizmos.DrawWireCube(startPos, GetComponent<Collider>().bounds.size);
            Gizmos.DrawWireCube(endPos, GetComponent<Collider>().bounds.size);
            Gizmos.DrawCube(endPos, Vector3.one * .2f);
        }
       

    }
}
[CustomEditor(typeof(Elevator))]
public class ElevatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
            return;
        Undo.RecordObject(target, "made changes to elevator");
        Elevator myScript = (Elevator)target;
        myScript.useFreeTransform = GUILayout.Toggle(myScript.useFreeTransform, "Use Free Transform");
        myScript.oneTimeUse = GUILayout.Toggle(myScript.oneTimeUse, "One Time Use");
        if (!myScript.useFreeTransform)
            DrawDefaultInspector();
        else
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Reset EndPos"))
            {
                myScript.endPos = myScript.transform.position + Vector3.one;
            }
        }

        
        if (GUILayout.Button("Flip Start Position"))
        {
            myScript.FlipStartPosition();
        }
    }
    public void OnSceneGUI()
    {
        var t = (target as Elevator);
        if (t.useFreeTransform)
        {
            Vector3 pos = Handles.PositionHandle(t.endPos, Quaternion.identity);
          
                Undo.RecordObject(target, "Free Move End Point");
                //t.endPosHandle = pos;
                t.endPos = pos.RoundUp() + Vector3.one * .5f;
                t.Update();
          
        }
        

    }
}