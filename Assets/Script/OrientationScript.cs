using UnityEngine;
using System.Collections;

public enum Orientation { up, down, left, right, front, back }

public static class OrientationScript
{
    public static Vector3[] OrientationVectors = new Vector3[]
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };

    public static Vector3 ToVector (this Orientation orientation)
    {
        return OrientationVectors[(int)orientation];
    }
    public static Orientation Inverse(this Orientation _orientation)
    {
        switch (_orientation)
        {
            case Orientation.up:
                return Orientation.down;
            case Orientation.down:
                return Orientation.up;
            case Orientation.left:
                return Orientation.right;
            case Orientation.right:
                return Orientation.left;
            case Orientation.front:
                return Orientation.back;
            case Orientation.back:
                return Orientation.front;
            default:
                return Orientation.down;
        }
    }
    public static Orientation RotateAroundUp(this Orientation orientation, bool clockwise)
    {
        if (orientation == Orientation.up || orientation == Orientation.down)
            return orientation;
        Orientation[] UpRotation = new Orientation[] { Orientation.front, Orientation.right, Orientation.back, Orientation.left };

        for (int i = 0; i < UpRotation.Length; i++)
        {
            if (orientation == UpRotation[i])
            {
                if (clockwise)
                    return UpRotation[(i + 1) % 4];
                else
                    return UpRotation[(i + 3) % 4];
            }
        }
        throw new System.Exception("Cannot rotate " + orientation + " around up");
    }
    public static Orientation RotateAroundRight(this Orientation orientation, bool clockwise)
    {
        if (orientation == Orientation.right || orientation == Orientation.left)
            return orientation;

        Orientation[] RightRotation = new Orientation[] { Orientation.front, Orientation.down, Orientation.back, Orientation.up };

        for (int i = 0; i < RightRotation.Length; i++)
        {
            if (orientation == RightRotation[i])
            {
                if (clockwise)
                    return RightRotation[(i + 1) % 4];
                else
                    return RightRotation[(i + 3) % 4];
            }
        }
        throw new System.Exception("Cannot rotate " + orientation + " around right");
    }
    public static Orientation RotateAroundFront(this Orientation orientation, bool clockwise)
    {
        if (orientation == Orientation.front || orientation == Orientation.back)
            return orientation;

        Orientation[] FrontRotation = new Orientation[] { Orientation.left, Orientation.down, Orientation.right, Orientation.up };

        for (int i = 0; i < FrontRotation.Length; i++)
        {
            if (orientation == FrontRotation[i])
            {
                if (clockwise)
                    return FrontRotation[(i + 1) % 4];
                else
                    return FrontRotation[(i + 3) % 4];
            }
        }
        throw new System.Exception("Cannot rotate " + orientation + " around front");
    }


    public static Orientation Vector3ToOrientation(this Vector3 _vector)
    {
        float minAngleFound = Mathf.Infinity;
        int minIndex = -1;

        for (int i = 0; i < OrientationVectors.Length; i++)
        {
            if (Vector3.Angle(_vector, OrientationVectors[i]) < minAngleFound)
            {
                minAngleFound = Vector3.Angle(_vector, OrientationVectors[i]);
                minIndex = i;
            }
        }
        return (Orientation)minIndex;

    }
    public static bool SameAxisAs(this Orientation orientation, Orientation otherOrientation)
    {

        int a = ((int)orientation);
        int b = ((int)otherOrientation);
        a /= 2;
        b /= 2;

        if (a == b)
            return true;
        return false;
    }
}