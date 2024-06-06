using UnityEngine;
using UnityEngine.AI;

public class WolfStateMachine : MonoBehaviour
{
    public static string sheepTag = "Sheep";
    [HideInInspector] public static GameObject[] sheeps;
    [HideInInspector] public static PoolSpawner spawner;

    public WolfIdleState idleState;
    public WolfWalkState walkState;
    public WolfRunState runState;
    public WolfStalkState stalkState;
    public WolfAttackState attackState;
    private WolfBaseState curState;

    [SerializeField] private float health = 10.0f;


    [SerializeField] public float walkSpeed;
    [SerializeField] public float viewDistance;
    [SerializeField] public LayerMask viewMask;
    [SerializeField] public float attackThreshhold;
    [SerializeField] public float attackRange;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Animator anim;
    [HideInInspector] public NavMeshAgent agent;

    void OnEnable()
    {
        UpdateSheepList();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();


        curState = idleState;
        curState.Enter(this);
    }

    public static void UpdateSheepList()
    {
        sheeps = spawner.referenceOfOtherSpawner.activeEnemyList.ToArray(); ;
    }

    void Update()
    {
        curState.Do(this);
        curState.CheckState(this);

        CheckHealth();
    }

    public void SwitchState(WolfBaseState _newState)
    {
        curState.Exit(this);
        curState = _newState;
        curState.Enter(this);
    }

    public GameObject GetNearestSheep(Vector3 _pos)
    {
        if (sheeps == null)
        {
            return null;
        }
        GameObject nearestSheep = sheeps[0];
        foreach (GameObject sheep in sheeps)
        {
            if (Vector3.Distance(_pos, nearestSheep.transform.position) < Vector3.Distance(_pos, sheep.transform.position))
            {
                nearestSheep = sheep;
            }
        }
        return nearestSheep;
    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            Die();
        }
    }
    public void DealDamage(float _damage)
    {
        health -= _damage;
        Debug.Log("Wolf Damage");

    }

    private void Die()
    {
        spawner.DespawnEnemy(this.gameObject);
    }
}
