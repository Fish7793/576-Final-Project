public class CanvasSenseBlock : CanvasBlockBase
{
    public override void Begin(CanvasGraph cg)
    {
        var sense = new SenseBlock();
        sense.sense = (v) => cg.Agent.Sense(v);
        Block = sense;
    }
}

