using System.Collections;
using UnityEngine;

public class FireworkFX : MonoBehaviour
{
    Rigidbody rb;
    TrailRenderer tr;
    ParticleSystem ps;
    public Material mat;
    public float forceMulti = 2;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
        ps = GetComponent<ParticleSystem>();

        var color = Random.ColorHSV(0, 1, 0.75f, 1);
        var mat_instace = new Material(mat);
        mat_instace.SetColor("_EmissionColor", color);

        tr.material = mat_instace;
        rb.AddForce(new Vector3(Random.value * 2 - 1, 0.75f + Random.value / 2, Random.value * 2 - 1) * forceMulti, ForceMode.Impulse);
        rb.AddTorque(new Vector3(0, Random.value * 2 - 1, 0) * 100, ForceMode.Impulse);

        ps.GetComponent<ParticleSystemRenderer>().material = mat_instace;
        ps.GetComponent<ParticleSystemRenderer>().trailMaterial = mat_instace;

        StartCoroutine(Routine());  
    }

    public IEnumerator Routine()
    {
        yield return new WaitForSeconds(0.4f + Random.value / 2);
        ps.Emit(50);
        tr.enabled = false;
        Destroy(gameObject, 2);
    }
}