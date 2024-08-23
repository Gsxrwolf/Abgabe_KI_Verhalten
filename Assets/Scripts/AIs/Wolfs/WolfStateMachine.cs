using UnityEngine;
using UnityEngine.AI;

public class WolfStateMachine : MonoBehaviour
{
    [HideInInspector] public static GameObject[] sheeps;
    [HideInInspector] public PoolSpawner spawner;

    [HideInInspector] public GameObject dog;

    public WolfIdleState wolfIdleState;
    public WolfRunState wolfRunState;
    public WolfWalkState wolfWalkState;
    public WolfStalkState wolfStalkState;
    public WolfAttackState wolfAttackState;
    public WolfEscapeState wolfEscapeState;
    private WolfBaseState curState;

    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] public float stalkSpeed;

    [HideInInspector] public Animator anim;
    [HideInInspector] public NavMeshAgent agent;



    [SerializeField] public float wolfSmellDistanceToHuntSheep = 40f;
    [SerializeField] public float wolfSlowDownDistance = 20f;
    [SerializeField] public float wolfHideDistance = 10f;
    [SerializeField] public float wolfAttackDistance = 2f;
    [SerializeField] public float wolfEscapedDistance = 7f;
    [SerializeField] public float wolfIsSaveAgainDistance = 20f;

    void OnEnable()
    {
        SheepStateMachine.UpdateSheepList += UpdateSheepList;
        PoolSpawner.RoundIsOver += RoundOver;
        dog = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.Warp(transform.position);

        agent.speed = walkSpeed;

        curState = wolfIdleState;
        curState.Enter(this);
    }
    private void OnDisable()
    {
        SheepStateMachine.UpdateSheepList -= UpdateSheepList;
        PoolSpawner.RoundIsOver -= RoundOver;
    }

    private void Start()
    {
        UpdateSheepList();
    }

    public void UpdateSheepList()
    {
        sheeps = spawner.referenceOfOtherSpawner.activeEnemyList.ToArray();
    }

    void Update()
    {
        if (agent.isOnNavMesh)
        {
            GetComponent<NavMeshAgent>().enabled = true;
        }
        RotateInRightDirection();

        anim.SetFloat("WalkSpeed", agent.velocity.magnitude);

        curState.Do(this);
        curState.CheckState(this);
    }

    private void RotateInRightDirection()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            Vector3 direction = -agent.velocity.normalized; //Einfach nicht fragen warum minus ich weiß es selbst nicht aber es funktioniert :)
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }
    }
    private void RoundOver()
    {
        SwitchState(wolfIdleState);
    }

    public void SwitchState(WolfBaseState _newState)
    {
        curState.Exit(this);
        curState = _newState;
        curState.Enter(this);
    }

    public GameObject GetNearestSheep(Vector3 _pos)
    {
        if (sheeps == null || sheeps.Length == 0)
        {
            return null;
        }
        GameObject nearestSheep = sheeps[0];
        foreach (GameObject sheep in sheeps)
        {
            if (Vector3.Distance(_pos, nearestSheep.transform.position) > Vector3.Distance(_pos, sheep.transform.position))
            {
                nearestSheep = sheep;
            }
        }
        return nearestSheep;
    }
}
