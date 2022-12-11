
using UnityEngine.UI;

public class CanvasPredicateBlock : CanvasBlockBase
{
    public ComparisonField field;

    public override void Begin(CanvasGraph cg)
    {
        var pred = new PredicateBlock();
        if (field != null)
        {
            pred.comparison = field.Value;
        }
        Block = pred;
    }
}

