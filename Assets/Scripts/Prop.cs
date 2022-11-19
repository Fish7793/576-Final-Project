using UnityEngine;

public class Prop : MonoBehaviour
{
    public GameObject prefab;

    public PropInfo GetInfo()
    {
        return new PropInfo(this);
    }
}

