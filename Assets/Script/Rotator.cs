using UnityEngine;
using System.Collections;

[AddComponentMenu("Interactables/Rotator")]
public class Rotator : InteractableBlock {
    [Min(1f)]
    [Tooltip("Degrees per second")]
    public float speed = 90;
    public int times = 1;
    [Tooltip("Animation reverses after one use, then back again")]
    public bool flipFlop = true;
    [Tooltip("Direction of the rotation")]
    public bool clockwise = true;
    public enum Direction { up, right, front};
    [Tooltip("Direction of movement")]
    public Direction direction = Direction.up;
    Vector3[] directionVectors = new[] { Vector3.up, Vector3.right, Vector3.forward};

    bool isRotating = false;
    public override void Start()
    {
        base.Start();
       
    }
    public override void Update()
    {
        base.Update(); 
    }

    public override void Activate()
    {
        base.Activate();
        if (!isRotating)
            StartCoroutine(Rotate(clockwise));
    }

    public IEnumerator Rotate(bool _clockwise)
    {
        for (int i = 0; i < times; i++)
        {
            isRotating = true;
            Vector3 rotationVector;

            switch (direction)
            {
                case Direction.up:
                    rotationVector = Vector3.up; break;
                case Direction.right:
                    rotationVector = Vector3.right; break;
                case Direction.front:
                    rotationVector = Vector3.forward; break;
                default:
                    rotationVector = Vector3.up; break;
            }

            Quaternion startRotation = transform.rotation;

            while (Quaternion.Angle(transform.rotation, startRotation) < (90))
            {
                transform.Rotate(rotationVector, _clockwise ? speed * Time.deltaTime : -speed * Time.deltaTime);    
                yield return null;
            }

            if (Quaternion.Angle(transform.rotation, startRotation) != 90)
            {
                if (_clockwise)
                    transform.Rotate(rotationVector, 90 - Quaternion.Angle(transform.rotation, startRotation));
                else
                    transform.Rotate(rotationVector, Quaternion.Angle(transform.rotation, startRotation) + 270);
            }
            if (flipFlop)
                clockwise = !clockwise;

            if (transform.childCount != 0)
            {
                if (direction == Direction.up)
                {
                    transform.GetChild(0).GetComponent<Seeker>().orientation.RotateAroundUp(_clockwise);
                    grid.targetOrientation = grid.targetOrientation.RotateAroundUp(_clockwise);
                }
                if (direction == Direction.right)
                {
                    transform.GetChild(0).GetComponent<Seeker>().orientation.RotateAroundRight(_clockwise);
                    grid.targetOrientation = grid.targetOrientation.RotateAroundRight(_clockwise);

                }
                if (direction == Direction.front)
                {
                    transform.GetChild(0).GetComponent<Seeker>().orientation.RotateAroundFront(_clockwise);
                    grid.targetOrientation = grid.targetOrientation.RotateAroundFront(_clockwise);

                }
            }
        }
        isRotating = false;
    }

    void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(transform.position, GridScript.OrientationVectors[(int)direction * 2], .7f);
        
        Gizmos.color = Color.blue;
        int cw = clockwise ? 1: -1; 

        MoreGizmos.DrawArrow(transform.position + directionVectors[((int)direction + 2) % 3]*.7f, directionVectors[((int)direction + 1) % 3], Color.blue, .8f*cw, 25, .2f);

        Gizmos.DrawLine(
            transform.position + GridScript.OrientationVectors[(int)direction * 2]*.75f, 
            transform.position - GridScript.OrientationVectors[(int)direction * 2]*.75f);
    }

}
