using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashFX : MonoBehaviour
{
    public int count = 20;
    void Start()
    {
        var ps = GetComponentInChildren<ParticleSystem>();
        ps.Emit(count);
        Destroy(gameObject, 1);
    }
}
