using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    Seeker seeker;
    GridScript grid;
    private void Start() {
        seeker = GetComponent<Seeker>();
        grid = seeker.grid;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.forward, out hit))
                if (hit.collider.GetComponent<Crate>())
                    hit.collider.GetComponent<Crate>().Push(Orientation.front);

            if(Physics.Raycast(transform.position, Vector3.back, out hit))
                if (hit.collider.GetComponent<Crate>())
                    hit.collider.GetComponent<Crate>().Push(Orientation.back);

            if(Physics.Raycast(transform.position, Vector3.left, out hit))
                if (hit.collider.GetComponent<Crate>())
                    hit.collider.GetComponent<Crate>().Push(Orientation.left);

            if(Physics.Raycast(transform.position, Vector3.right, out hit))
                if (hit.collider.GetComponent<Crate>())
                    hit.collider.GetComponent<Crate>().Push(Orientation.right);

        }
    }

    public void OnStop() {

    }

    void FindActions() {

    }
}
