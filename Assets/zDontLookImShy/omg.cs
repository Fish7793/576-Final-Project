using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class omg : MonoBehaviour
{
    public Transform prisim;
    public float speeeeeeeeeed = 10;
    public Vector3 otherDirrrrrrrrrrrrrrr;
    public float otherSpeeeeeeeeeeeeed;
    public float why = 0;
    public float bruh = 0;
    public Vector2 som = new Vector2(30, 60);

    public void Start()
    {
        bruh = som[0] + Random.value * som[1];
        otherSpeeeeeeeeeeeeed = 0.25f + Random.value * 0.4f;
        why = Random.value * 5 + 1;
        otherDirrrrrrrrrrrrrrr = new Vector3(Random.value * 2 - 1, 0, Random.value * 2 - 1) * 100;
    }

    void Update()
    {
        prisim.eulerAngles += new Vector3(0, speeeeeeeeeed, 0) * Time.deltaTime;
        transform.position = otherDirrrrrrrrrrrrrrr * (Time.time * otherSpeeeeeeeeeeeeed - bruh) + new Vector3(0, why, 0);
    }
}
