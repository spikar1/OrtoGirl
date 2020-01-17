using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Snap)), CanEditMultipleObjects]
public class SnapEditor : Editor{

    private void OnSceneGUI() {
        Snap snap = (Snap)target;
        if (snap.extendedEditor == false)
            return;



        //Handles.ArrowHandleCap(0, Zposition, Quaternion.Euler(Vector3.right), 1, EventType.ContextClick);

        //Rotation around X axis. Button along Z axis.
        Vector3 Zposition = snap.transform.localPosition + Vector3.up * 1f;
        float size = .5f;
        float pickSize = size * 2f;

        if (Handles.Button(Zposition, Quaternion.identity, size, pickSize, Handles.CircleHandleCap)) {
            Debug.Log("The button was pressed!");
            snap.transform.Rotate(Vector3.back, 90, Space.World);
        }

        //Rotation around Y axis. Button along X axis.
        Vector3 Xposition = snap.transform.localPosition + Vector3.right * 1f;

        if (Handles.Button(Xposition, Quaternion.Euler(Vector3.right * 90), size, pickSize, Handles.CircleHandleCap)){
            Debug.Log("The button was pressed!");
            snap.transform.Rotate(Vector3.down, 90, Space.World);
        }

        //Rotation around X axis. Button along Z axis.
        Vector3 Yposition = snap.transform.localPosition + Vector3.forward * 1f;

        if (Handles.Button(Yposition, Quaternion.Euler(Vector3.up * 90), size, pickSize, Handles.CircleHandleCap)) {
            Debug.Log("The button was pressed!");
            snap.transform.Rotate(Vector3.left, 90, Space.World);
        }
    }

    




}
