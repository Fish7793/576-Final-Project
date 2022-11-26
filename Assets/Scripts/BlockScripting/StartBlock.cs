

public class StartBlock : Block
{
    public override bool Evaluate()
    {
        return false;
    }

    public override void Reset()
    {
        value = new Value(null);
    }
}