using System.Collections;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public GameObject prefab;
    public PropType[] propTags;

    public virtual IEnumerator Step()
    {
        yield return null;
    }

    public virtual PropInfo GetInfo()
    {
        return new PropInfo(this);
    }
}