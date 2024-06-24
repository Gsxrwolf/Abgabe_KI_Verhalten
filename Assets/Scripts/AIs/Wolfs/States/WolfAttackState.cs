using UnityEngine;

public class WolfAttackState : WolfBaseState
{
    private WolfStateMachine context;


    private bool isAttacking;
    public override void Enter(WolfStateMachine _context)
    {
        _context.anim.SetTrigger("Attack");
        context = _context;

        context.agent.speed = 0.01f;

        isAttacking = true;
    }
    public override void Do(WolfStateMachine _context)
    {
    }
    public void StartJumpAttack()
    {
    }
    public void CheckHit()
    {
        GameObject target = context.GetNearestSheep(transform.position);

        if(Vector3.Distance(transform.position,target.transform.position) < 3f)
        {
            target.GetComponent<SheepStateMachine>().DealDamage();
        }
    }

    private void EndJumpAttack()
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
            _context.SwitchState(_context.idleState);
        }
    }
    public override void Exit(WolfStateMachine _context)
    {
    }
}
