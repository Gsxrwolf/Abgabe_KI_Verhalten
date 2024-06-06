using Unity.Mathematics;
using UnityEngine;

public class SheepIdleState : SheepBaseState
{

    private SheepStateMachine context;

    [SerializeField] private float maxXValueForDestination;
    [SerializeField] private float minXValueForDestination;
    [SerializeField] private float maxZValueForDestination;
    [SerializeField] private float minZValueForDestination;


    private float timer;
    [SerializeField] private float switchDirectionIntervall = 10;
    public override void Enter(SheepStateMachine _context)
    {
        context = _context;
        System.Random rnd = new System.Random();
        switchDirectionIntervall += rnd.Next(2);
    }
    private void NavigateRandomDestination()
    {
        if (context.agent.isOnNavMesh)
        {
            context.anim.SetTrigger("Walk");
            context.agent.SetDestination(GetRandomDestination());
        }
    }
    private Vector3 GetRandomDestination()
    {
        Vector3 destination;
        System.Random rnd = new System.Random();

        float angle = (float)(rnd.NextDouble() * Mathf.PI * 2); 
        destination = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

        destination *= rnd.Next(20);

        Vector3 targetPos = transform.position + destination;

        if (InvalidDestination(targetPos))
        {
            return GetRandomDestination();
        }

        return targetPos;
    }
    private bool InvalidDestination(Vector3 _destination)
    {
        if (_destination.x > maxXValueForDestination)
            return true;
        if (_destination.x < minXValueForDestination)
            return true;
        if (_destination.z > maxZValueForDestination)
            return true;
        if (_destination.z < minZValueForDestination)
            return true;

        return false;
    }
    public override void Do(SheepStateMachine _context)
    {
        timer += Time.deltaTime;
        if (timer > switchDirectionIntervall)
        {
            timer = 0;
            NavigateRandomDestination();
        }
    }
    public override void FixedDo(SheepStateMachine _context)
    {
    }
    public override void CheckState(SheepStateMachine _context)
    {
    }
    public override void Exit(SheepStateMachine _context)
    {
        _context.anim.ResetTrigger("Walk");
    }

}
