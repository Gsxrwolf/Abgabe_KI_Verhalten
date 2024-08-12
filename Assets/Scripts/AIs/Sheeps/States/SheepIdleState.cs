using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SheepIdleState : SheepBaseState
{

    private SheepStateMachine context;


    private float timer;
    private float switchDirectionIntervall = 5;
    [SerializeField] private int switchDirectionIntervallMin = 3;
    [SerializeField] private int switchDirectionIntervallMax = 7;


    private float breedTimer;
    private float breedTime = 10;
    [SerializeField] private int breedTimeMin = 8;
    [SerializeField] private int breedTimeMax = 12;
    private bool breedingTime = false;
    public override void Enter(SheepStateMachine _context)
    {
        context = _context;
        System.Random rnd = new System.Random();
        switchDirectionIntervall = rnd.Next(switchDirectionIntervallMin, switchDirectionIntervallMax);
        breedTime = rnd.Next(breedTimeMin,breedTimeMax);
        _context.NavigateRandomDestination();
    }
    public override void Do(SheepStateMachine _context)
    {
        timer += Time.deltaTime;
        if (timer > switchDirectionIntervall)
        {
            timer = 0;
            _context.NavigateRandomDestination();
        }
        breedTimer += Time.deltaTime;
        if (breedTimer > breedTime)
        {
            breedTimer = 0;
            breedingTime = true;
        }
    }
    public override void FixedDo(SheepStateMachine _context)
    {
    }
    public override void CheckState(SheepStateMachine _context)
    {
        List<GameObject> visibleWolves = _context.CheckFOV(typeof(WolfStateMachine));
        if(visibleWolves.Count > 0)
            _context.SwitchState(_context.sheepRunState);
        if(breedingTime)
            _context.SwitchState(_context.sheepFindPartnerState);
    }
    public override void Exit(SheepStateMachine _context)
    {
        breedingTime = false;
    }

}
