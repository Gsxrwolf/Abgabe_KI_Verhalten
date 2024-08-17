using UnityEngine;

public class WolfWalkState : WolfBaseState
{
    public override void Enter(WolfStateMachine _context)
    {
    }
    public override void Do(WolfStateMachine _context)
    {
        _context.anim.SetBool("Walk", true);
        _context.agent.speed = _context.walkSpeed;
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
        if (Vector3.Distance(nearestSheep.transform.position, transform.position) > _context.wolfSlowDownDistance)
        {
            _context.SwitchState(_context.wolfrunState);
        }
        if (Vector3.Distance(nearestSheep.transform.position, transform.position) < _context.wolfHideDistance)
        {
            _context.SwitchState(_context.wolfstalkState);
        }
    }
    public override void Exit(WolfStateMachine _context)
    {
        _context.anim.SetBool("Walk", false);
    }
}
