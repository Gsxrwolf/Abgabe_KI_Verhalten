using UnityEngine;
using UnityEngine.UIElements;

public class WolfIdleState : WolfBaseState
{
    private WolfStateMachine context;

    [SerializeField] private float maxXValueForDestination;
    [SerializeField] private float minXValueForDestination;
    [SerializeField] private float maxYValueForDestination;
    [SerializeField] private float minYValueForDestination;

    [SerializeField] private float switchDirectionIntervall = 10;
    public override void Enter(WolfStateMachine _context)
    {
        context = _context;
        InvokeRepeating("NavigateRandomDestination", 0f, switchDirectionIntervall);
    }
    private void NavigateRandomDestination()
    {
        context.anim.SetTrigger("Walk");
        context.agent.SetDestination(GetRandomDestination());
    }
    private Vector3 GetRandomDestination()
    {
        Vector3 destination;
        System.Random rnd = new System.Random();

        destination = new Vector3(rnd.Next(20), 0, rnd.Next(20));

        destination.Normalize();

        destination *= rnd.Next();

        destination += transform.position;

        if(InvalidDestination(destination))
        {
            return GetRandomDestination();
        }

        return destination;
    }
    private bool InvalidDestination(Vector3 _destination)
    {
        if(_destination.x > maxXValueForDestination)
            return true;
        if(_destination.x < minXValueForDestination)
            return true;
        if(_destination.y > maxYValueForDestination)
            return true;
        if(_destination.y < minYValueForDestination) 
            return true;

        return false;
        
    }
    public override void Do(WolfStateMachine _context)
    {
        
    }
    public override void FixedDo(WolfStateMachine _context)
    {
    }
    public override void CheckState(WolfStateMachine _context)
    {
        GameObject nearestSheep = _context.GetNearestSheep(transform.position);
        if(Vector3.Distance(nearestSheep.transform.position,transform.position) < _context.wolfSmellDistanceToHuntSheep)
        {
            _context.SwitchState(_context.runState);
        }
    }
    public override void Exit(WolfStateMachine _context)
    {
        CancelInvoke("NavigateRandomDestination");
        _context.anim.ResetTrigger("Walk");
    }

}
