using System.Collections;
using System.Collections.Generic;
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
        GameObject nearestSheep = _context.GetNearestSheep(transform.position);
        if (Vector3.Distance(nearestSheep.transform.position, transform.position) > _context.wolfslowDownDistance)
        {
            _context.SwitchState(_context.runState);
        }
        if (Vector3.Distance(nearestSheep.transform.position, transform.position) < _context.wolfhideDistance)
        {
            _context.SwitchState(_context.stalkState);
        }
    }
    public override void Exit(WolfStateMachine _context)
    {
        _context.anim.SetBool("Walk", false);
    }
}
