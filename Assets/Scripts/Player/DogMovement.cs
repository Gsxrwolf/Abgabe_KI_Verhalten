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

    [SerializeField] private float normalSpeed;
    [SerializeField] private float followingSpeed;

    [SerializeField] private float maxFollowDistance;


    private GameObject targetWolf;
    void Start()
    {
        CamController.newDogDestination += SetDogDestination;
        CamController.followWolf += FollowWolf;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;
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
            agent.SetDestination(targetWolf.transform.position);
            agent.speed = followingSpeed;
        }
    }

    private void SetDogDestination(Vector3 _newDes)
    {
        following = false;
        agent.SetDestination(_newDes);
        agent.speed = normalSpeed;
    }
    private void FollowWolf(GameObject _targetWolf)
    {
        following = true;
        targetWolf = _targetWolf;
    }
}
