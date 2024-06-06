using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepStateMachine: MonoBehaviour
{
    public string sheepTag = "Sheep";
    [HideInInspector] public GameObject[] sheeps;
    [HideInInspector] public static PoolSpawner spawner;

    private SheepBaseState curState;

    [SerializeField] private float health = 10.0f;


    [SerializeField] public float walkSpeed;
    [SerializeField] public float viewDistance;
    [SerializeField] public LayerMask viewMask;
    [SerializeField] public float attackThreshhold;
    [SerializeField] public float attackRange;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sr;
    private Animator anim;
    public Vector3 scale;

    void OnEnable()
    {
        sheeps = GameObject.FindGameObjectsWithTag(sheepTag);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();


        //curState = sheepIdleState;
        curState.Enter(this); 
    }

    void Update()
    {
        curState.Do(this);
        curState.CheckState(this);

        CheckHealth();
    }

    public void SwitchState(SheepBaseState _newState)
    {
        curState.Exit(this);
        curState = _newState;
        curState.Enter(this);
    }

    private void CheckHealth()
    {
        if(health <= 0)
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
        WolfStateMachine.UpdateSheepList();
    }
}
