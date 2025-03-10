using UnityEngine;

public class WolfRunState : WolfBaseState
{
    public override void Enter(WolfStateMachine _context)
    {
    }
    public override void Do(WolfStateMachine _context)
    {
        _context.anim.SetTrigger("Run");
        _context.agent.speed = _context.runSpeed;
        GameObject nearestSheep = _context.GetNearestSheep(transform.position);
        _context.agent.SetDestination(nearestSheep.transform.position); 
    }
    public override void FixedDo(WolfStateMachine _context)
    {
    }
    public override void CheckState(WolfStateMachine _context)
    {
        if (Vector3.Distance(_context.dog.transform.position, transform.position) < _context.wolfEscapedDistance)
        {
            _context.SwitchState(_context.wolfEscapeState);
            return;
        }
        GameObject nearestSheep = _context.GetNearestSheep(transform.position);
        if (Vector3.Distance(nearestSheep.transform.position, transform.position) > _context.wolfSmellDistanceToHuntSheep)
        {
            _context.SwitchState(_context.wolfIdleState);
        }
        if (Vector3.Distance(nearestSheep.transform.position, transform.position) < _context.wolfSlowDownDistance)
        {
            _context.SwitchState(_context.wolfWalkState);
        }
    }
    public override void Exit(WolfStateMachine _context)
    {
        _context.anim.ResetTrigger("Run");
    }
}
