

using UnityEngine;

public class CanvasActionBlock : CanvasBlockBase
{
    public BlockActionType actionType;

    public override void Begin(CanvasGraph cg)
    {
        var action = new ActionBlock();
        switch (actionType)
        {
            case BlockActionType.None:
                break;
            case BlockActionType.MoveForward:
                action.action = () => cg.Agent.Move();
                break;
            case BlockActionType.RotateLeft:
                action.action = () => cg.Agent.Rotate(-90);
                break;
            case BlockActionType.RotateRight:
                action.action = () => cg.Agent.Rotate(90);
                break;
            case BlockActionType.Jump:
                action.action = () => cg.Agent.Jump();
                break;
            case BlockActionType.Attack:
                action.action = () => cg.Agent.Attack();
                break;
        }
        Block = action;
    }
}

public class CanvasStartBlock : CanvasBlockBase
{
    public override void Begin(CanvasGraph cg)
    {
        Block = new StartBlock();
    }
}

public class CanvasSenseBlock : CanvasBlockBase
{
    public override void Begin(CanvasGraph cg)
    {
        Block = new SenseBlock();
    }
}

public enum BlockInputType
{
    None, f1, f2, f3, boolean
}

public class CanvasInputBlock : CanvasBlockBase
{
    public BlockInputType inputType;
    public UnityEngine.UI.Dropdown typeDropdown;
    public MonoBehaviour inputField;

    public void Start()
    {
        
    }

    public void UpdateInput()
    {
        switch (inputType)
        {

        }
    }

    public override void Begin(CanvasGraph cg)
    {
        object value = null;

        if (inputField!= null)
        {
            switch (inputField)
            {
                case UnityEngine.UI.Toggle toggle:
                    break;
                case UnityEngine.UI.InputField inputField:
                    break;
            }
        }

        Block = new InputBlock() { value = new Value(value) };
    }
}

public class CanvasBranchBlock : CanvasBlockBase
{
    public override void Begin(CanvasGraph cg)
    {
        Block = new BranchBlock();
    }
}

public class CanvasPredicateBlock : CanvasBlockBase
{
    public override void Begin(CanvasGraph cg)
    {
        Block = new PredicateBlock();
    }
}

