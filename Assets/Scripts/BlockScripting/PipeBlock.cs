using System.Linq;

public class PipeBlock : Block
{
    public override bool Evaluate()
    {
        value = inputs.FirstOrDefault()?.value;
        return false;
    }

    public override void Reset()
    {
        value = new Value(null);
    }
}