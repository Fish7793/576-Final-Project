public class PlayerAgent : Agent
{
    public override void SetAnimatorState()
    {
        animator.SetBool("Roll_Anim", false);
        animator.SetBool("Walk_Anim", state == AgentState.Walking);
        animator.SetBool("Open_Anim", state != AgentState.Turning && state != AgentState.Jumping);
        animator.SetBool("Jump_Anim", state == AgentState.Jumping);
    }
}
