using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : Prop
{
    public Vector3Int positionTarget;
    public Vector3 eulerAngleTarget;

    public float moveSpeed = 1;
    public float rotationSpeed = 0.5f;

    public System.Func<Vector3Int, bool> moveCheck;
    public System.Func<Vector3Int, bool> isGround;
    public System.Func<Vector3Int, PropType[]> sense;
    public System.Func<Agent, bool> stoppingCondition;
    public bool stopped = false;

    public AgentState state;
    public Animator animator;

    void Start()
    {
        positionTarget = transform.position.ToVector3Int();
        eulerAngleTarget = transform.eulerAngles;
    }

    void Update()
    {
        if (!stopped)
        {
            var delta = positionTarget - transform.position;
            delta = new Vector3(delta.x, 0, delta.z);
            if (delta.magnitude > 0.05)
                transform.position += delta.normalized * moveSpeed * Time.deltaTime;

            var rdelta = Mathf.DeltaAngle(eulerAngleTarget.y - 180, transform.eulerAngles.y);
            if (Mathf.Abs(rdelta + 180) > 2)
                transform.eulerAngles += rotationSpeed * Time.deltaTime * new Vector3(0, rdelta, 0);

            //animator.SetInteger("state", (int)state);
            animator.SetBool("Roll_Anim", false);
            animator.SetBool("Walk_Anim", state == AgentState.Walking);
            animator.SetBool("Open_Anim", state != AgentState.Turning);
        }
    }

    void SetState(AgentState state)
    {
        if (this != null)
            this.state = state;
    }

    void CheckStoppingCondition()
    {
        if (!stopped)
            stopped = stoppingCondition != null && stoppingCondition.Invoke(this);
    }

    public void Move()
    {
        state = AgentState.Walking;
        StartCoroutine(GameManager.Delay(() => SetState(AgentState.Idle), 1));
        if (isGround != null && !isGround.Invoke(positionTarget + transform.forward.ToVector3Int()))
            gameObject.GetComponent<Rigidbody>().useGravity = true;

        //if (moveCheck != null && moveCheck.Invoke(positionTarget + transform.forward.ToVector3Int()))
        //    positionTarget += transform.forward.ToVector3Int();

        positionTarget += transform.forward.ToVector3Int();
        CheckStoppingCondition();
    }

    public void Rotate(float amount)
    {
        state = AgentState.Turning;
        StartCoroutine(GameManager.Delay(() => SetState(AgentState.Idle), 1));
        eulerAngleTarget = new Vector3(0, (eulerAngleTarget.y + amount) % 360f, 0);
        CheckStoppingCondition();
    }

    public void Attack()
    {
        Debug.Log("Attacking! >:)");
        CheckStoppingCondition();
    }

    public void Jump()
    {
        Debug.Log("Jumping! :O");
        CheckStoppingCondition();
    }

    public void PickUp()
    {
        Debug.Log("Grabbing! :3");
        CheckStoppingCondition();
    }

    public void Use()
    {
        Debug.Log("Using! :)");
        CheckStoppingCondition();
    }

    public void Die()
    {
        if (gameObject != null)
        {
            var i = Instantiate(GameManager.prefabs["ExplodeFX"], transform);
            i.transform.SetParent(null);
            i.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
            stopped = true;
            Destroy(gameObject);
        }
    }

    public PropType[] Sense(Vector3Int localOffset)
    {
        CheckStoppingCondition();
        var res = transform.forward * localOffset[0] + transform.up * localOffset[1] + transform.right * localOffset[2];
        print(positionTarget + res.ToVector3Int());
        return sense(positionTarget + res.ToVector3Int());
    }
}

public enum AgentState
{
    Idle=0, Walking=1, Turning=2, Jumping=3, Attacking=4, Using=5, Dying=6, Ragdoll=7
}