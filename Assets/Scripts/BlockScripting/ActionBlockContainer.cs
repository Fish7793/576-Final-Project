using System.Collections.Generic;
using System.Linq;

public class ActionBlockContainer : ActionBlock
{
    public List<ActionBlock> blocks;
    
    public override bool Evaluate()
    {
        return blocks.Any(x => x.Evaluate());
    }

    public override void Reset()
    {

    }
}
