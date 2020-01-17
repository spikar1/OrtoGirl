using UnityEngine;
using System.Collections;

public class Slope : Block {

    public Orientation downOrientation = Orientation.down;
    public Orientation faceOrientation = Orientation.back;


    // Use this for initialization
    public override void Start() {
        base.Start();
        faceOrientation = transform.forward.Vector3ToOrientation();
        downOrientation = transform.up.Vector3ToOrientation().Inverse();

    }

    // Update is called once per frame
    void Update() {

    }

    void OnDrawGizmos() {
        faceOrientation = transform.forward.Vector3ToOrientation();
        downOrientation = transform.up.Vector3ToOrientation().Inverse();
        if (downOrientation.SameAxisAs(faceOrientation)) {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, Vector3.one);
            throw new System.Exception("Cant have Down and Face orientation on the same axis");
        }



    }
}
