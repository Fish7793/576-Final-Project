using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public System.Action setAnimatorState;

    public bool stopped = false;
    public bool updatePos = true;

    public AgentState state;
    public Animator animator;

    void Start()
    {
        positionTarget = transform.position.ToVector3Int();
        eulerAngleTarget = transform.eulerAngles;
    }

    public virtual void SetAnimatorState()
    {

    }

    void Update()
    {
        if (!stopped && updatePos)
        {
            var delta = positionTarget - transform.position;
            delta = new Vector3(delta.x, 0, delta.z);
            if (delta.magnitude > 0.05)
                transform.position += delta.normalized * moveSpeed * Time.deltaTime;

            var rdelta = Mathf.DeltaAngle(eulerAngleTarget.y - 180, transform.eulerAngles.y);
            if (Mathf.Abs(rdelta + 180) > 2)
                transform.eulerAngles += rotationSpeed * Time.deltaTime * new Vector3(0, rdelta, 0);

            if (isGround != null && !isGround.Invoke(positionTarget))
                gameObject.GetComponent<Rigidbody>().useGravity = true;
        }

        SetAnimatorState();
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
        updatePos = true;
        state = AgentState.Walking;
        StartCoroutine(GameManager.Delay(() => 
        {
            CheckStoppingCondition();
            SetState(AgentState.Idle); 
        }, 1));
        if (isGround != null && !isGround.Invoke(positionTarget + transform.forward.ToVector3Int()))
            gameObject.GetComponent<Rigidbody>().useGravity = true;

        //if (moveCheck != null && moveCheck.Invoke(positionTarget + transform.forward.ToVector3Int()))
        //    positionTarget += transform.forward.ToVector3Int();

        positionTarget += transform.forward.ToVector3Int();
    }

    public void Rotate(float amount)
    {
        state = AgentState.Turning;
        StartCoroutine(GameManager.Delay(() => SetState(AgentState.Idle), 1));
        eulerAngleTarget = new Vector3(0, (eulerAngleTarget.y + amount) % 360f, 0);
    }

    public void Attack()
    {
        Debug.Log("Attacking! >:)");
        CheckStoppingCondition();
    }

    public IEnumerator IEJump()
    {
        for (float i = 1/60f; i < 1; i += 1 / 60f)
        {
            if (gameObject != null)
            {
                transform.position += transform.forward * 2 / 60f + transform.up * Mathf.Sin(2 * Mathf.PI * i) * 2 / 60f;
                yield return new WaitForSeconds(1 / 60f);
            }
        }
    }

    public void Jump()
    {
        state = AgentState.Jumping;
        StartCoroutine(GameManager.Delay(() => 
        { 
            SetState(AgentState.Idle);
            positionTarget = transform.position.ToVector3Int();
            updatePos = true;
            CheckStoppingCondition();
        }, 1));
        StartCoroutine(IEJump());

        updatePos = false;
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
