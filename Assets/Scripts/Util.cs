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

    public static string ToString2(this PropType[] p)
    {
        string s = "";
        foreach (var prop in p)
        {
            s += string.Format("{0}, ", prop.ToString());
        }

        return s.Trim();
    }
}