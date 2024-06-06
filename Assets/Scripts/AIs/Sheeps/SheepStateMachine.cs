using UnityEngine;
using UnityEngine.AI;

public class SheepStateMachine : MonoBehaviour
{
    [HideInInspector] public static PoolSpawner spawner;

    public SheepIdleState sheepIdleState;
    public SheepRunState sheepRunState;
    public SheepHungerState sheepHungerState;
    public SheepEatState sheepEatState;
    public SheepFindPartnerState sheepFindPartnerState;
    public SheepBreedState sheepBreedState;
    private SheepBaseState curState;

    [SerializeField] private float health = 10.0f;


    [SerializeField] public float walkSpeed;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Animator anim;
    [HideInInspector] public NavMeshAgent agent;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();


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

    private void Die()
    {
        spawner.DespawnEnemy(this.gameObject);
        WolfStateMachine.UpdateSheepList();
    }
}
