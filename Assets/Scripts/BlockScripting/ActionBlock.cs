public class ActionBlock : Block
{
    public System.Action action;

    public override bool Evaluate()
    {
        action?.Invoke();
        return true;
    }

    public override void Reset()
    {

    }
}
