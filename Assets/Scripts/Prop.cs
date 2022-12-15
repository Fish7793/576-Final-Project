using System.Collections;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public GameObject prefab;
    public PropType[] propTags;

    public virtual void Step()
    {

    }

    public virtual PropInfo GetInfo()
    {
        return new PropInfo(this);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.name.ToLower().Contains("water"))
        {
            var i = Instantiate(GameManager.prefabs["SplashFX"], transform);
            i.transform.SetParent(null);
        }
    }
}
