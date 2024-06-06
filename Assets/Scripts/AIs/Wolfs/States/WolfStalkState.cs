using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfStalkState : WolfBaseState
{
    public override void Enter(WolfStateMachine _context)
    {
    }
    public override void Do(WolfStateMachine _context)
    {
        _context.anim.SetTrigger("Stalk");
        GameObject nearestSheep = _context.GetNearestSheep(transform.position);
        _context.agent.SetDestination(nearestSheep.transform.position);
    }
    public override void FixedDo(WolfStateMachine _context)
    {
    }
    public override void CheckState(WolfStateMachine _context)
    {
        GameObject nearestSheep = _context.GetNearestSheep(transform.position);
        if (Vector3.Distance(nearestSheep.transform.position, transform.position) > _context.wolfhideDistance)
        {
            _context.SwitchState(_context.walkState);
        }
        if (Vector3.Distance(nearestSheep.transform.position, transform.position) < _context.wolfattackDistance)
        {
            _context.SwitchState(_context.attackState);
        }
    }
    public override void Exit(WolfStateMachine _context)
    {
        _context.anim.ResetTrigger("Stalk");
    }
}
