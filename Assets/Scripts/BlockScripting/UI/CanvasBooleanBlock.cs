public class CanvasBooleanBlock : CanvasBlockBase
{
    public BooleanOpField field;

    public override void Begin(CanvasGraph cg)
    {
        var b = new BooleanBlock();
        if (field != null)
        {
            b.operation = field.Value;
        }
        Block = b;
        Block.value = new Value(false);
    }
}
