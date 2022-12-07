using UnityEngine;

public class InputBlock : Block
{
    public BlockInputType type;
    public override bool Evaluate()
    {
        return false;
    }

    public override void Reset()
    {

    }
}
