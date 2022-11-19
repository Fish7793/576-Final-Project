using UnityEngine;

public static class Util
{
    public static Vector3Int ToVector3Int(this Vector3 v)
    {
        return new Vector3Int(
                Mathf.RoundToInt(v.x),
                Mathf.RoundToInt(v.y),
                Mathf.RoundToInt(v.z)
            );
    }
}