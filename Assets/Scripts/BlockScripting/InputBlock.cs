public class InputBlock : Block
{
    public InputType type;
    public override bool Evaluate()
    {
        switch (type)
        {
            case InputType.None:
                value = new Value(null);
                break;
            case InputType.SenseProp:
                value = new Value(null);
                break;
            case InputType.Direction:
                value = new Value(null);
                break;
        }

        return false;
    }

    public override void Reset()
    {
        
    }
}
