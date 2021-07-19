using UnityEngine;

public struct Utility
{
    public static float DistanceToVec3(Vector3 start, Vector3 end)
    {
        return (end - start).magnitude;
    }
}

