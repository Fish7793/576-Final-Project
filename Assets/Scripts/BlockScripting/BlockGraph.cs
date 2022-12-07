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
    public List<Block> blocks = new List<Block>();

    public void Evaluate()
    {
        var others = blocks.Where(x => x.GetType() != typeof(ActionBlock)); 

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
        foreach (var block in others)
        {
            block.Evaluate();
        }
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
