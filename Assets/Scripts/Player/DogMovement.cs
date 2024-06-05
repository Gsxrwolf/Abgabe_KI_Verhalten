using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class DogMovement : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;

    private bool following;
    [SerializeField] private float maxFollowDistance;
    [SerializeField] private float attackRange;


    private GameObject targetWolf;
    void Start()
    {
        CamController.newDogDestination += SetDogDestination;
        CamController.followWolf += FollowWolf;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        anim = GetComponent<Animator>();
    }
    private void OnDisable()
    {
        CamController.newDogDestination -= SetDogDestination;
        CamController.followWolf -= FollowWolf;
    }

    private void Update()
    {
        if(following)
        {
            if(Vector3.Distance(transform.position, targetWolf.transform.position) > maxFollowDistance)
            {
                return;
            }
            if(Vector3.Distance(transform.position, targetWolf.transform.position) < attackRange)
            {
                Attack();
            }
            agent.SetDestination(targetWolf.transform.position);
        }
    }

    private void Attack()
    {
        following = false;
    }

    private void SetDogDestination(Vector3 _newDes)
    {
        agent.SetDestination(_newDes);
    }
    private void FollowWolf(GameObject _targetWolf)
    {
        following = true;
        targetWolf = _targetWolf;
    }
}
