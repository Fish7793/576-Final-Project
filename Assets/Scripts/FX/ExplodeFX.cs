using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeFX : MonoBehaviour
{
    public int count = 100;
    public GameObject sphere;
    public Material sphereMat;

    public AnimationCurve sphereSizeCurve;
    public float sphereSizeScale= 2;
    public Gradient sphereColorCurve;
    public float t = 2;

    void Start()
    {
        var sfx = transform.position.y < -0.15 ? "explosion2" : "explosion1";
        var src = AudioManager.Play(GameManager.sounds[sfx], pitch: (Random.value * 2 - 1) / 10 + 1);
        src.transform.SetParent(transform);
        src.transform.localPosition = Vector3.zero;
        src.spatialBlend = 0.5f;
        src.volume = 0.8f;
        sphereMat = sphere.GetComponent<Renderer>().material;
        var ps = GetComponentInChildren<ParticleSystem>();
        ps.Emit(count);
        StartCoroutine(AnimateSphere());
        Destroy(gameObject, 3);
    }

    public IEnumerator AnimateSphere()
    {
        for (float i = 0; i < t; i += 1 / 60f)
        {
            sphere.transform.localScale = Vector3.one * sphereSizeCurve.Evaluate(i / t) * sphereSizeScale;
            sphereMat.SetColor("_Color", sphereColorCurve.Evaluate(i / t));
            yield return new WaitForSeconds(1 / 60f);
        }

        sphere.SetActive(false);
    }
}
