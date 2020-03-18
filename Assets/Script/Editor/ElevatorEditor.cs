using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Elevator))]
public class ElevatorEditor : Editor {
    public override void OnInspectorGUI() {
        if (Application.isPlaying)
            return;
        Undo.RecordObject(target, "made changes to elevator");
        Elevator myScript = (Elevator)target;
        myScript.useFreeTransform = GUILayout.Toggle(myScript.useFreeTransform, "Use Free Transform");
        myScript.oneTimeUse = GUILayout.Toggle(myScript.oneTimeUse, "One Time Use");
        if (!myScript.useFreeTransform)
            DrawDefaultInspector();
        else {
            DrawDefaultInspector();
            if (GUILayout.Button("Reset EndPos")) {
                myScript.endPos = myScript.transform.position + Vector3.one;
            }
        }


        if (GUILayout.Button("Flip Start Position")) {
            myScript.FlipStartPosition();
        }
    }
    public void OnSceneGUI() {
        var t = (target as Elevator);
        if (t.useFreeTransform) {
            Vector3 pos = Handles.PositionHandle(t.endPos, Quaternion.identity);

            Undo.RecordObject(target, "Free Move End Point");
            //t.endPosHandle = pos;
            t.endPos = pos.RoundUp() + Vector3.one * .5f;
            t.Update();

        }


    }
}