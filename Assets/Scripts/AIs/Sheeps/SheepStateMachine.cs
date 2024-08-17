using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SheepStateMachine : MonoBehaviour
{
    [HideInInspector] public static PoolSpawner spawner;

    public SheepIdleState sheepIdleState;
    public SheepRunState sheepRunState;
    public SheepFindPartnerState sheepFindPartnerState;
    public SheepBreedState sheepBreedState;
    private SheepBaseState curState;
    public SheepBaseState CurState { get { return curState; } }

    [SerializeField] public float walkSpeed;
    [SerializeField] public const float viewRange = 5f;
    [Range(0, 360)][SerializeField] public const float viewAngle = 80f;

    [SerializeField] private float maxXValueForDestination = 575;
    [SerializeField] private float minXValueForDestination = 500;
    [SerializeField] private float maxZValueForDestination = 565;
    [SerializeField] private float minZValueForDestination = 515;

    [HideInInspector] public Animator anim;
    [HideInInspector] public NavMeshAgent agent;


    public static event Action UpdateSheepList;

    void OnEnable()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.speed = walkSpeed;

        curState = sheepIdleState;
        curState.Enter(this);
    }

    void Update()
    {
        curState.Do(this);
        curState.CheckState(this);
    }

    public void SwitchState(SheepBaseState _newState)
    {
        curState.Exit(this);
        curState = _newState;
        curState.Enter(this);
    }
    public void DealDamage()
    {
        Die();
        Debug.Log("Sheep got damage");
    }


    public void NavigateRandomDestination()
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(GetRandomDestination());
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
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return GetRandomDestination();
        }
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

    public List<GameObject> CheckFOV(string _tagToSearch, float _viewRange = viewRange, float _viewAngle = viewAngle)
    {
        List<GameObject> hits = new List<GameObject>();

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, _viewRange, Vector3.zero);

        foreach (RaycastHit hit in rayHits)
        {
            if (hit.collider.CompareTag(_tagToSearch))
            {
                Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, directionToTarget) < _viewAngle / 2)
                {
                    hits.Add(hit.collider.gameObject);
                }
            }
        }
        return hits;
    }
    public List<GameObject> CheckFOV(System.Type _componentType, float _viewRange = viewRange, float _viewAngle = viewAngle)
    {
        List<GameObject> hits = new List<GameObject>();

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, _viewRange, Vector3.zero);

        foreach (RaycastHit hit in rayHits)
        {
            if (hit.collider.TryGetComponent(_componentType, out Component temp))
            {
                Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, directionToTarget) < _viewAngle / 2)
                {
                    hits.Add(hit.collider.gameObject);
                }
            }
        }
        return hits;
    }

    public GameObject GetNearestFromAll(GameObject[] _listOfAll)
    {
        GameObject nearest = _listOfAll.First();
        foreach (GameObject _object in _listOfAll)
        {
            if (Vector3.Distance(transform.position, _object.transform.position) < Vector3.Distance(transform.position, nearest.transform.position))
                nearest = _object;
        }
        return nearest;
    }
    public GameObject GetNearestFromAll(List<GameObject> _listOfAll)
    {
        GameObject nearest = _listOfAll.First();
        foreach (GameObject _object in _listOfAll)
        {
            if (Vector3.Distance(transform.position, _object.transform.position) < Vector3.Distance(transform.position, nearest.transform.position))
                nearest = _object;
        }
        return nearest;
    }
    public static Vector3 GetMiddlePoint(Vector3 _a, Vector3 _b)
    {
        return _a + ((_b - _a) / 2);
    }


    private void Die()
    {
        spawner.DespawnEnemy(this.gameObject);
        UpdateSheepList?.Invoke();
    }
}
