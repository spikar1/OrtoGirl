    using UnityEngine;
using UnityEditor;
using System.Collections;

public class Snap : MonoBehaviour
{

    //[MenuItem("MyMenu/Do Something _p")]
    //static void DoSomething()
    //{

    //}

    public bool useParent = false;

    public Transform parent;
    
    public bool snapPosition = true;

    public bool snapRotation = true;

    public bool snapScale = true;

    public bool extendedEditor = false;

    public bool snapToGround = false;

    private float gridSize = 1;
    float gridFactor;

    Vector3 inputPos;
    Vector3 newSnapPosition = new Vector3(0, 0, 0);
    Vector3 newSnapScale = new Vector3(0, 0, 0);

    Collider col;
    Event currentEvent;

    void Awake()
    {
        enabled = false;
        
    }


    public void OnGUI()
    {
        currentEvent = Event.current;
        Debug.Log(Event.GetEventCount());
        if (currentEvent.button == 0 && currentEvent.isMouse)
        {
            Debug.Log("left mouse button clicked");
        }
    }

    void Show() {
        print("halla");
    }

    void OnDrawGizmosSelected()
    {
        if (extendedEditor) {
            //Draw Arrows for the Editor extension, should be fixed to be in the editor script
            //Z rotation
            MoreGizmos.DrawArrow(Vector3.up * 1.5f + transform.position, Vector3.right, .5f, 20, .3f);
            //X rotation
            MoreGizmos.DrawArrow(Vector3.forward * 1.5f + transform.position, Vector3.up, .5f, 20, .3f);
            //Y rotation
            MoreGizmos.DrawArrow(Vector3.right * 1.5f + transform.position, Vector3.forward, .5f, 20, .3f);
        }
        

        //Debug.Log(Event.current);
        /* if (currentEvent.button == 0 && currentEvent.isMouse)
         {
             Debug.Log("left mouse button clicked");
         }*/

        if (enabled == false)
            return;

        if (Input.GetKey(KeyCode.G))
        {
            print("things");
        }

        if (gridSize != 0)
            gridFactor = 1 / gridSize;
        else
            gridFactor = 1;

        inputPos = transform.localPosition;
        col = GetComponent<Collider>();

        //make input values easier to work with
        newSnapPosition = Vector3Extensions.Round(10 * transform.localPosition) / (10);
        newSnapScale = transform.localScale.Round();
        if (newSnapScale.x % gridSize != 0)
        {
            newSnapScale.x -= newSnapScale.x % gridSize;
        }
        if (newSnapScale.y % gridSize != 0)
        {
            newSnapScale.y -= newSnapScale.y % gridSize;
        }
        if (newSnapScale.z % gridSize != 0)
        {
            newSnapScale.z -= newSnapScale.z % gridSize;
        }

        //Calculate new positions with snap in mind
        newSnapPosition.x = FindSnapPosition(newSnapPosition.x, newSnapScale.x, inputPos.x);
        newSnapPosition.y = FindSnapPosition(newSnapPosition.y, newSnapScale.y, inputPos.y);
        newSnapPosition.z = FindSnapPosition(newSnapPosition.z, newSnapScale.z, inputPos.z);


        //Make sure no object is flat in any axis
        if (newSnapScale.x == 0)
            newSnapScale.x = gridSize;
        if (newSnapScale.y == 0)
            newSnapScale.y = gridSize;
        if (newSnapScale.z == 0)
            newSnapScale.z = gridSize;



        Vector3 gizmoPos;
        Vector3 gizmoScale;

        Gizmos.color = Color.red; //Color of the wireCube.

        //If snap is toggled on, apply transformation changes. Otherwise use original values.
        if (snapPosition)
        {
            transform.localPosition = newSnapPosition;
            gizmoPos = transform.position;
        }
        else
            gizmoPos = transform.position;

        if (snapRotation)
        {
            //snap rotation code goes here
        }

        if (snapScale)
        {

            transform.localScale = newSnapScale;
            gizmoScale = newSnapScale;
        }
        else
            gizmoScale = col.bounds.size;

        Gizmos.DrawWireCube(gizmoPos, gizmoScale);


        //float scale = Handles.ScaleValueHandle(5, Vector3.zero, Quaternion.identity, 3, Handles.ArrowCap, .5f);


        /* if (snapRotation)
         {*/
        Vector3 newRot;
        newRot.x = Mathf.Round(transform.localRotation.eulerAngles.x / 90) * 90;
        newRot.y = Mathf.Round(transform.localRotation.eulerAngles.y / 90) * 90;
        newRot.z = Mathf.Round(transform.localRotation.eulerAngles.z / 90) * 90;
        transform.localRotation = Quaternion.Euler(newRot);
        //}
    }

    public static float FindSnapPosition(float pos, float scale, float refPos)
    {
        if (scale.Odd())
        {
            //print (scale * gridFactor);
            pos -= pos % .5f;
            if ((Mathf.Abs(pos - .5f) % 1 == .5f))
            {
                pos += FloatExtensions.Sign(refPos) * .5f;
            }

        }
        else if ((scale).Even())
        {
            //print("its even");
            pos = Mathf.Round(pos);
        }
        return pos;
    }

    public static Vector3 FindSnapPosition(Vector3 position, Vector3 scale) {


        var newSnapPosition = Vector3Extensions.Round(10 * position) / (10);
        var newSnapScale = scale.Round();
        if (newSnapScale.x % 1 != 0) {
            newSnapScale.x -= newSnapScale.x % 1;
        }
        if (newSnapScale.y % 1 != 0) {
            newSnapScale.y -= newSnapScale.y % 1;
        }
        if (newSnapScale.z % 1 != 0) {
            newSnapScale.z -= newSnapScale.z % 1;
        }



        position.x = Snap.FindSnapPosition(newSnapPosition.x, newSnapScale.x, position.x);
        position.y = Snap.FindSnapPosition(newSnapPosition.y, newSnapScale.y, position.y);
        position.z = Snap.FindSnapPosition(newSnapPosition.z, newSnapScale.z, position.z);

        return position;
    }



}