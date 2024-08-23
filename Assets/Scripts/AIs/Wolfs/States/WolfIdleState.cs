using UnityEngine;
using UnityEngine.AI;

public class WolfIdleState : WolfBaseState
{
    private WolfStateMachine context;

    [SerializeField] private float maxXValueForDestination = 630;
    [SerializeField] private float minXValueForDestination = 450;
    [SerializeField] private float maxZValueForDestination = 630;
    [SerializeField] private float minZValueForDestination = 480;




    private float timer;
    private float switchDirectionIntervall = 5;
    [SerializeField] private int switchDirectionIntervallMin = 3;
    [SerializeField] private int switchDirectionIntervallMax = 7;

    [SerializeField] private int maxDestinationDistance = 20;
    public override void Enter(WolfStateMachine _context)
    {
        context = _context;
        context.agent.speed = context.walkSpeed;
        System.Random rnd = new System.Random();
        switchDirectionIntervall = rnd.Next(switchDirectionIntervallMin, switchDirectionIntervallMax);
        NavigateRandomDestination();
    }
    private void NavigateRandomDestination()
    {
        if (context.agent.isOnNavMesh && context.agent.enabled == true)
        {
            context.agent.SetDestination(GetRandomDestination());
        }
    }

    int errorCounter = 0;
    private Vector3 GetRandomDestination()
    {
        Vector3 destination;
        System.Random rnd = new System.Random();

        float angle = (float)(rnd.NextDouble() * Mathf.PI * 2);
        destination = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

        destination *= rnd.Next(maxDestinationDistance);

        Vector3 targetPos = transform.position + destination;
        if (errorCounter > 100)
        {
            errorCounter = 0;
            context.spawner.DespawnEnemy(gameObject);
            return Vector3.zero;
        }
        if (InvalidDestination(targetPos))
        {
            errorCounter++;
            return GetRandomDestination();
        }
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }


        return Vector3.zero;
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
    public override void Do(WolfStateMachine _context)
    {
        timer += Time.deltaTime;
        if (timer > switchDirectionIntervall)
        {
            timer = 0;
            NavigateRandomDestination();
        }
        if (_context.agent.velocity.magnitude > 0.1f)
        {
            _context.anim.SetBool("Walk", true);
        }
        else
        {
            _context.anim.SetBool("Walk", false);
        }
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
        if (nearestSheep == null) return;

        if (Vector3.Distance(nearestSheep.transform.position, transform.position) < _context.wolfSmellDistanceToHuntSheep)
        {
            _context.SwitchState(_context.wolfRunState);
        }
    }
    public override void Exit(WolfStateMachine _context)
    {
        _context.anim.SetBool("Walk", false);
    }

}
