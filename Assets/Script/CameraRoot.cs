using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoot : MonoBehaviour {


    public bool rotating = false;
    

	void Update () {
        /*if (Input.GetKeyDown(KeyCode.E) && !rotating) {
            rotating = true;
            Rotate(true);
        }
            
        else if (Input.GetKeyDown(KeyCode.Q) && !rotating) {
            rotating = true;
            Rotate(false);
        }*/
        if (Input.GetMouseButton(1)) {
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * 2);

            transform.GetChild(0).Rotate(Vector3.right, -Input.GetAxis("Mouse Y"));
        }
        if (Input.GetMouseButton(2)) {
            transform.position += transform.right * -Input.GetAxis("Mouse X") * .4f;
            transform.position += transform.forward * -Input.GetAxis("Mouse Y") * .4f;
            
        }
        transform.GetChild(0).GetChild(0).position += transform.GetChild(0).GetChild(0).forward * Input.GetAxis("Mouse ScrollWheel") * 5;
    }

    private void LateUpdate() {
        
    }

    void Rotate(bool ccw) {
        StartCoroutine(RotateLoop(ccw));
    }

    IEnumerator RotateLoop(bool ccw) {
        Vector3 targetRot = ccw ?
            transform.rotation.eulerAngles + Vector3.up * 90 :
            transform.rotation.eulerAngles + Vector3.up * -90;
        targetRot.y = targetRot.y % 360;

        print(targetRot);
        int i = 0;
        while (true) 
        {
            
            i++;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRot), i);

            /*if (ccw) {
                transform.Rotate(Vector3.up, .9f);
            }
            if (!ccw) {
                transform.Rotate(Vector3.up, -.9f);
            }*/
            if (Vector3.Distance(transform.rotation.eulerAngles, targetRot) < 10f || Vector3.Distance(transform.rotation.eulerAngles, targetRot) == 360) {
                transform.rotation = Quaternion.Euler(targetRot);
                rotating = false;
                StopAllCoroutines();
            }
            yield return new WaitForSeconds(.01f);
        }
    }
}
