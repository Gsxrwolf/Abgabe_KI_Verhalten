using UnityEngine;

public class WolfAttackState : WolfBaseState
{
    private WolfStateMachine context;

    private bool isAttacking;
    public override void Enter(WolfStateMachine _context)
    {
        _context.anim.SetTrigger("Attack");
        context = _context;
        isAttacking = true;
    }
    public override void Do(WolfStateMachine _context)
    {
    }
    public void CheckHit()
    {
    }

    private void EndAttack()
    {
        isAttacking = false;
        context.anim.ResetTrigger("Attack");
    }
    public override void FixedDo(WolfStateMachine _context)
    {
    }
    public override void CheckState(WolfStateMachine _context)
    {
        if(!isAttacking)
        {
            _context.SwitchState(_context.stalkState);
        }
    }
    public override void Exit(WolfStateMachine _context)
    {
    }
}
