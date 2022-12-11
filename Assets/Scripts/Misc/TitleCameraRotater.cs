using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCameraRotater : MonoBehaviour
{
    public float speed = 5;
    public float sinScale = 1;

    public Transform arm;
    public float armSinScale = 1;
    public float armSinOffset = 0.5f;
    public float armSinSpeed = 0.5f;

    void Update()
    {
        transform.eulerAngles += new Vector3(0, speed + Mathf.Sin(Time.time) * sinScale, 0) * Time.deltaTime;
        arm.transform.eulerAngles += new Vector3(Mathf.Sin(Time.time * armSinSpeed + armSinOffset) * armSinScale, 0, 0) * Time.deltaTime;
    }
}
