using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/** 
 *  Blocks that control logic of player agent
 *  Action blocks take 1 tick, everything else
 *  is instant.
 */
public class BlockGraph
{
    public Block start;
    public Block current;
    public List<Block> blocks = new();
    bool ordererd = false;
    public List<Block> Order()
    {
        Debug.Log("Sorting graph execution!");
        List<Block> result = new();
        HashSet<Block> visited = new();
        Queue<Block> front = new();

        foreach (var b in blocks)
            front.Enqueue(b);

        int attempts = 0;
        while(front.Count > 0 && attempts < 1000)
        {
            Block cur = front.Dequeue();
            if (visited.Contains(cur))
            {
                continue;
            }

            if (cur.inputs.All(x => visited.Contains(x)) || cur.inputs.Count == 0)
            {
                result.Add(cur);
                visited.Add(cur);
            }
            else
            {
                front.Enqueue(cur);
                attempts++;
            }
        }

        ordererd = true;
        Debug.Log(string.Format("Execution sorted in {0} attempts!", attempts));
        return result;
    }

    public void Evaluate()
    {
        if (!ordererd)
            blocks = Order();

        var others = blocks.Where(x => x.GetType() != typeof(ActionBlock));
        foreach (var block in others)
        {
            Debug.Log(block);
            block.Evaluate();
        }

        bool res;
        int iter = 0;

        do
        {
            if (current == null)
                current = start;
            Debug.Log(current);
            res = current.Evaluate();
            current = current?.next;
            iter++;
        } while (!res && iter < 1000);
    }

    public void Reset()
    {
        current = null;
        foreach (var block in blocks)
        {
            block.Reset();
        }
    }
}
