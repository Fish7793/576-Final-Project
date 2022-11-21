using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public Vector3Int positionTarget;
    public Vector3 eulerAngleTarget;

    public float moveSpeed = 1;
    public float rotationSpeed = 0.5f;

    public System.Func<Vector3Int, bool> moveCheck;

    void Start()
    {
        positionTarget = transform.position.ToVector3Int();
        eulerAngleTarget = transform.eulerAngles;
    }

    void Update()
    {
        var delta = positionTarget - transform.position;
        transform.position += delta * moveSpeed * Time.deltaTime;

        var rdelta = Mathf.DeltaAngle(eulerAngleTarget.y - 180, transform.eulerAngles.y);
        transform.eulerAngles += rotationSpeed * Time.deltaTime * new Vector3(0, rdelta, 0);
    }

    public void Move()
    {
        if (moveCheck != null && moveCheck.Invoke(positionTarget + transform.forward.ToVector3Int()))
            positionTarget += transform.forward.ToVector3Int();
    }

    public void Rotate(float amount)
    {
        eulerAngleTarget = new Vector3(0, (eulerAngleTarget.y + amount) % 360f, 0);
    }

    public void Attack()
    {
        Debug.Log("Attacking! >:)");
    }

    public void Jump()
    {
        Debug.Log("Jumping! :O");
    }

    public void PickUp()
    {
        Debug.Log("Grabbing! :3");
    }

    public void Use()
    {
        Debug.Log("Using! :)");
    }

    public PropType Sense(Vector3 dir)
    {
        return PropType.None;
    }
}
