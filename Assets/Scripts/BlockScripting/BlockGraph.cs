using System.Collections.Generic;

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
        bool res;
        do
        {
            if (current == null)
                current = start;

            res = current.Evaluate();
            UnityEngine.Debug.Log(current);
            current = current?.next;
        } while (!res);
        
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
