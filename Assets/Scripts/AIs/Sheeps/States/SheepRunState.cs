using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepRunState : SheepBaseState
{

    private Vector3 startDirection;
    private Vector3 runDestination;

    [SerializeField] private float runDistance;
    [SerializeField] private float runSpeed;

    public override void Enter(SheepStateMachine _context)
    {
        Debug.Log("Run");
        startDirection = transform.rotation.eulerAngles.normalized;
        runDestination = new Vector3(-startDirection.x* runDistance, startDirection.y, -startDirection.z* runDistance);

        _context.agent.speed = runSpeed;
        _context.agent.SetDestination(runDestination);
    }
    public override void Do(SheepStateMachine _context)
    {
    }
    public override void FixedDo(SheepStateMachine _context)
    {
    }
    public override void CheckState(SheepStateMachine _context)
    {
        if (_context.agent.pathStatus == NavMeshPathStatus.PathComplete)
            _context.SwitchState(_context.sheepIdleState);
    }
    public override void Exit(SheepStateMachine _context)
    {
        _context.agent.speed = _context.walkSpeed;
    }
}
