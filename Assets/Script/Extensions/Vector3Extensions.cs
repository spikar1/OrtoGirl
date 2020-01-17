using UnityEngine;
using System.Collections;

public static class Vector3Extensions {

	public static Vector3 RoundUp (this Vector3 v3)
	{
		v3 = new Vector3(
			v3.x > 0 ? (int)v3.x : v3.x,
            v3.y > 0 ? (int)v3.y : (int)v3.y,
            v3.z > 0 ? (int)v3.z : (int)v3.z);

		return v3;

	}
    public static Vector3 Round(this Vector3 vector3)
    {
        vector3 = new Vector3(
            Mathf.RoundToInt(vector3.x),
            Mathf.RoundToInt(vector3.y),
            Mathf.RoundToInt(vector3.z));

        return vector3;

    }

    public static Vector3 Abs(this Vector3 vector3) {
        vector3 = new Vector3(
            Mathf.Abs(vector3.x),
            Mathf.Abs(vector3.y),
            Mathf.Abs(vector3.z));

        return vector3;
    }

}