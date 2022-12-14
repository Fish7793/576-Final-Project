using System.Collections.Generic;

public class BooleanBlock : Block
{
    readonly Dictionary<BooleanOp, System.Func<bool, bool, bool>> operations = new()
    {
        { BooleanOp.AND,    (a, b) => a && b },
        { BooleanOp.OR,     (a, b) => a || b },
        { BooleanOp.XOR,    (a, b) => !(a && b) && (a || b) },
    };

    public Block a;
    public Block b;
    public BooleanOp operation;

    public override bool Evaluate()
    {
        if (a != null && b != null && a.value.TryGetValue<bool>(out bool ba) && b.value.TryGetValue<bool>(out bool bb))
        {
            
            value = new Value(operations[operation](ba, bb));
        }
        return false;
    }

    public override void Reset()
    {
        value = new Value(false);
    }
}