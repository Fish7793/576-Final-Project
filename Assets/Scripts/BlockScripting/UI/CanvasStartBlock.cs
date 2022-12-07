public class CanvasStartBlock : CanvasBlockBase
{
    public override void Begin(CanvasGraph cg)
    {
        Block = new StartBlock();
    }
}