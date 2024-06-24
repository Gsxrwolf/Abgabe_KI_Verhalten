using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SheepFindPartnerState : SheepBaseState
{
    List<GameObject> availablePartners = new List<GameObject>();
    [HideInInspector] public GameObject partner;

    [SerializeField] private float smellPartnerDistance;


    private float timer;
    [SerializeField] private float switchDirectionIntervall = 3;

    public override void Enter(SheepStateMachine _context)
    {
        InvokeRepeating("UpdateAvailablePartners", 0f, 2f);
    }
    private void UpdateAvailablePartners()
    {
        List<GameObject> allPartners = new List<GameObject>(SheepStateMachine.spawner.activeEnemyList);

        allPartners.Remove(this.gameObject);
        foreach (GameObject partner in allPartners)
        {
            if (partner.GetComponent<SheepStateMachine>().CurState.GetType() == typeof(SheepFindPartnerState))
            {
                availablePartners.Add(partner);
            }
        }
    }
    public override void Do(SheepStateMachine _context)
    {

        timer += Time.deltaTime;
        if (timer > switchDirectionIntervall)
        {
            timer = 0;
            _context.NavigateRandomDestination();
        }
        if (availablePartners.Count != 0)
        {
            float temp = Vector3.Distance(_context.transform.position, _context.GetNearestFromAll(availablePartners).transform.position);
            if (Vector3.Distance(_context.transform.position,_context.GetNearestFromAll(availablePartners).transform.position) < smellPartnerDistance)
            {
                partner = _context.GetNearestFromAll(availablePartners);
            }
        }
        if (partner != null)
        {
            _context.agent.SetDestination(SheepStateMachine.GetMiddlePoint(transform.position, partner.transform.position));
        }
    }
    public override void FixedDo(SheepStateMachine _context)
    {
    }
    public override void CheckState(SheepStateMachine _context)
    {
        List<GameObject> visibleWolves = _context.CheckFOV(typeof(WolfStateMachine));
        if (visibleWolves.Count > 0)
            _context.SwitchState(_context.sheepRunState);
        if (partner != null && _context.agent.pathStatus == NavMeshPathStatus.PathComplete)
            _context.SwitchState(_context.sheepBreedState);
    }
    public override void Exit(SheepStateMachine _context)
    {
        CancelInvoke();
    }
}
