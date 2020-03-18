using UnityEngine;
using System.Collections;

public static class MoreGizmos {

	public static void DrawArrow (Vector3 startPos, Vector3 direction, Color color , float length,float arrowAngle, float headScale)
    {
        Gizmos.color = color;

        Gizmos.DrawRay(startPos, direction * length);
        Vector3 left =  Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowAngle, 0) * new Vector3(0, 0, 1);
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowAngle, 0) * new Vector3(0, 0, 1);
        Vector3 up = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + arrowAngle, 0, 0) * new Vector3(0, 0, 1);
        Vector3 down = Quaternion.LookRotation(direction) * Quaternion.Euler(180 - arrowAngle, 0, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(startPos + direction * length , left * length * headScale);
        Gizmos.DrawRay(startPos + direction * length , right * length * headScale);
        Gizmos.DrawRay(startPos + direction * length , up * length * headScale);
        Gizmos.DrawRay(startPos + direction * length , down * length * headScale);


    }
    public static void DrawArrow(Vector3 startPos, Vector3 direction, float length, float arrowAngle, float headScale)
    {
        Gizmos.DrawRay(startPos, direction * length);
        Vector3 left    = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowAngle, 0) * new Vector3(0, 0, 1);
        Vector3 right   = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowAngle, 0) * new Vector3(0, 0, 1);
        Vector3 up      = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + arrowAngle, 0, 0) * new Vector3(0, 0, 1);
        Vector3 down    = Quaternion.LookRotation(direction) * Quaternion.Euler(180 - arrowAngle, 0, 0) * new Vector3(0, 0, 1);    
        Gizmos.DrawRay(startPos + direction * length , left * length * headScale);
        Gizmos.DrawRay(startPos + direction * length , right * length * headScale);
        Gizmos.DrawRay(startPos + direction * length , up * length * headScale);
        Gizmos.DrawRay(startPos + direction * length , down * length * headScale);
    }
}
