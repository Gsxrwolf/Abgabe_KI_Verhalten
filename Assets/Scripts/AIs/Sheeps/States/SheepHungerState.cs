using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SheepHungerState : SheepBaseState
{

    public override void Enter(SheepStateMachine _context)
    {
        
    }
    public override void Do(SheepStateMachine _context)
    {
    }
    public override void FixedDo(SheepStateMachine _context)
    {
    }
    public override void CheckState(SheepStateMachine _context)
    {
        List<GameObject> visibleWolves = _context.CheckFOV(typeof(WolfStateMachine));
        if (visibleWolves.Count > 0)
            _context.SwitchState(_context.sheepRunState);
    }
    public override void Exit(SheepStateMachine _context)
    {
    }

}
