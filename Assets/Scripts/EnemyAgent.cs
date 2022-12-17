using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAgent : Agent
{
    public Func<Agent> getPlayer;
    public Func<Vector3Int, float> pathCost;
    public float explosionForce = 2;

    List<Vector3Int> GetNextVectors(Vector3Int v)
    {
        return new List<Vector3Int>() {
            v + new Vector3Int(1, 0, 0),
            v + new Vector3Int(-1, 0, 0),
            v + new Vector3Int(0, 0, 1),
            v + new Vector3Int(0, 0, -1)
        };
    }

    public List<Vector3Int> AStar(Vector3Int start, Vector3Int goal, Func<Vector3Int, float> h)
    {
        Dictionary<Vector3Int, float> cost = new();
        SortedList<float, Vector3Int> front = new();
        Dictionary<Vector3Int, Vector3Int> parents = new();

        Func<List<Vector3Int>> reconstruct = new Func<List<Vector3Int>>(() =>
        {
            var result = new List<Vector3Int>();
            var cur = goal;
            result.Add(cur);
            while (parents.ContainsKey(cur) && parents[cur] != start)
            {
                cur = parents[cur];
                result.Add(cur);
            }
            result.Reverse();
            return result;
        });

        front.Add(0, start);
        cost.Add(start, 0);

        int attempts = 0;
        while (front.Count > 0 && attempts < 512)
        {
            var curKv = front.First();
            front.RemoveAt(0);
            Vector3Int cur = curKv.Value;
            if (cur == goal)
            {
                return reconstruct();
            }

            foreach (Vector3Int v in GetNextVectors(cur))
            {
                float vCost = cost[cur] + (float)pathCost?.Invoke(v);
                if (!cost.ContainsKey(v) || vCost < cost[v])
                {
                    parents[v] = cur;
                    cost[v] = vCost;
                    if (front.ContainsValue(v))
                        front.RemoveAt(front.IndexOfValue(v));
                    float temp = vCost + h(v);
                    int at2 = 0;
                    while (front.ContainsKey(temp) && at2++ < 1000)
                    {
                        temp += UnityEngine.Random.value / 1000f;
                    }
                    front.Add(temp, v);
                }
            }

            attempts++;
        }

        print(String.Format("A* failed in {0} attempts!", attempts));
        return new List<Vector3Int>();
    }

    List<Vector3Int> path = new();
    public override void Step()
    {
        var player = getPlayer?.Invoke();
        if (player != null && !player.stopped)
        {
            path = AStar(transform.position.ToVector3Int(),
                            player.transform.position.ToVector3Int(),
                            (v) => (player.transform.position.ToVector3Int() - transform.position).sqrMagnitude
                        );
            
            if (path.Count > 0)
            {
                var delta = path.First() - transform.position.ToVector3Int();
                var sum = (positionTarget + transform.forward).ToVector3Int();
                print(String.Format("{0}, {1}", path.First(), sum));
                if (sum == path.First())
                {
                    if (isGround != null && isGround.Invoke(path.First()))
                        Move();
                }
                else
                {
                    var lsum = (positionTarget + transform.right).ToVector3Int();
                    if (lsum == path.First())
                        Rotate(90);
                    else
                        Rotate(-90);
                }
            }
        }
    }

    public void FixedUpdate()
    {
        for (int i = 0; i < path?.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.magenta);
        }
    }

    public override void SetAnimatorState()
    {
        animator.SetBool("walk", state == AgentState.Walking);
        animator.SetBool("turn_left", state == AgentState.Turning && Mathf.DeltaAngle(transform.eulerAngles.y, eulerAngleTarget.y) > 0);
        animator.SetBool("turn_right", state == AgentState.Turning && Mathf.DeltaAngle(transform.eulerAngles.y, eulerAngleTarget.y) < 0);
    }
    public void OnCollisionEnter(Collision collision)
    {
        print(collision);
        if (collision.gameObject.TryGetComponent(out Agent a)
         && a.propTags.Contains(PropType.Player))
        {
            Explode();
            a.stopped = true;
            var rb = a.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.AddForce((-(transform.position - collision.transform.position).normalized / 3 + new Vector3(0, 1, 0)) * explosionForce, ForceMode.Impulse);
            rb.AddTorque(new Vector3(UnityEngine.Random.value * 2 - 1, UnityEngine.Random.value * 2 - 1, UnityEngine.Random.value * 2 - 1) / 3, ForceMode.Impulse);
            GameManager.instance.StartCoroutine(GameManager.Delay(() => { if (a != null) a.Die(); }, 1));
        }
    }

    public void Explode()
    {
        var i = Instantiate(GameManager.prefabs["ExplodeFX"], transform);
        i.transform.SetParent(null);
        i.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
        Destroy(gameObject);
    }
}
