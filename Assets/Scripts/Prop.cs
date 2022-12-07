using System.Collections;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public GameObject prefab;
    public PropType[] propTags;

    public IEnumerator Step()
    {
        yield return null;
    }

    public PropInfo GetInfo()
    {
        return new PropInfo(this);
    }
}