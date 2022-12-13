public class CanvasPipeBlock : CanvasBlockBase
{
    public override void Begin(CanvasGraph cg)
    {
        Block = new PipeBlock() { value = new Value(null) };
    }
}
