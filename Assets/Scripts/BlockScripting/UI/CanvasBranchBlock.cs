
public class CanvasBranchBlock : CanvasBlockBase
{
    public override void Begin(CanvasGraph cg)
    {
        Block = new BranchBlock();
    }
}