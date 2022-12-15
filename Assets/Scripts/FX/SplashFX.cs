using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashFX : MonoBehaviour
{
    public int count = 20;
    void Start()
    {
        var src = AudioManager.Play(GameManager.sounds["splash"], pitch: (Random.value * 2 - 1) / 10 + 1);

        var ps = GetComponentInChildren<ParticleSystem>();
        ps.Emit(count);
        Destroy(gameObject, 1);
    }
}
