public class BranchBlock : Block
{
    public Block predicate;
    public Block ifTrue;
    public Block ifFalse;

    public override bool Evaluate()
    {
        if (predicate != null && predicate.value.TryGetValue(out bool b))
        {
            next = b ? ifTrue : ifFalse;
        }
        else
        {
            next = ifFalse != null ? ifFalse : ifTrue != null ? ifTrue : null;
        }

        return false;
    }

    public override void Reset()
    {
        next = null;
    }
}
