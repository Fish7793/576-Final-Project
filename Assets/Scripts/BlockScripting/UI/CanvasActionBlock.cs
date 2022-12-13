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
                action.action = () => cg.Agent?.Move();
                break;
            case BlockActionType.RotateLeft:
                action.action = () => cg.Agent?.Rotate(-90);
                break;
            case BlockActionType.RotateRight:
                action.action = () => cg.Agent?.Rotate(90);
                break;
            case BlockActionType.Jump:
                action.action = () => cg.Agent?.Jump();
                break;
            case BlockActionType.Attack:
                action.action = () => cg.Agent?.Attack();
                break;
        }
        Block = action;
    }
}

