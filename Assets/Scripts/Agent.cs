using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public Vector3Int positionTarget;
    public Vector3 eulerAngleTarget;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {

    }

    public void Rotate()
    {

    }

    public void Attack()
    {

    }

    public void Jump()
    {

    }

    public PropType Sense(Vector3 dir)
    {
        return PropType.None;
    }
}

public enum PropType
{
    None, Wall, Enemy, Bomb, Projectile
}
