using UnityEngine;

public class WolfEscapeState : WolfBaseState
{
    public override void Enter(WolfStateMachine _context)
    {
        _context.anim.SetTrigger("Run");
        _context.agent.speed = _context.runSpeed;
        Vector3 dogVector = transform.position - _context.dog.transform.position;
        Vector3 targetPos = transform.position + dogVector.normalized * _context.wolfIsSaveAgainDistance;
        _context.agent.SetDestination(targetPos);
        GameManager.Instance.AddPointsForWolfSpawn();
    }
    public override void Do(WolfStateMachine _context)
    {
    }
    public override void FixedDo(WolfStateMachine _context)
    {
    }
    public override void CheckState(WolfStateMachine _context)
    {
        if (!_context.agent.pathPending && (!_context.agent.hasPath))
        {
            _context.SwitchState(_context.wolfidleState);
        }
    }
    public override void Exit(WolfStateMachine _context)
    {
        _context.anim.SetBool("Walk", false);
    }
}
