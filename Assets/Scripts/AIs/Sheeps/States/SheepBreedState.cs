using UnityEngine;

public class SheepBreedState : SheepBaseState
{
    [SerializeField] private float babySpawnDelay;
    [HideInInspector] public bool lockBabySpawn;
    [HideInInspector] public bool breedingDone;

    private GameObject partner;
    private SheepStateMachine context;
    public override void Enter(SheepStateMachine _context)
    {
        context = _context;
        partner = context.sheepFindPartnerState.partner;
        Invoke("SpawnBabySheep", babySpawnDelay);
    }
    private void SpawnBabySheep()
    {
        if (!lockBabySpawn)
        {
            if (Vector3.Distance(transform.position, partner.transform.position) < context.sheepFindPartnerState.smellPartnerDistance)
            {
                partner.GetComponent<SheepBreedState>().lockBabySpawn = true;
                Vector3 spawnPos = SheepStateMachine.GetMiddlePoint(transform.position, partner.transform.position);
                SheepStateMachine.spawner.SpawnNewEnemy(spawnPos);

                breedingDone = true;
                partner.GetComponent<SheepBreedState>().breedingDone = true;

            }
            else
            {
                breedingDone = true;
                partner.GetComponent<SheepBreedState>().breedingDone = true;
            }

        }
    }
    public override void Do(SheepStateMachine _context)
    {
    }
    public override void FixedDo(SheepStateMachine _context)
    {
    }
    public override void CheckState(SheepStateMachine _context)
    {
        if (breedingDone)
            _context.SwitchState(_context.sheepIdleState);
    }
    public override void Exit(SheepStateMachine _context)
    {

        breedingDone = false;
        _context.sheepFindPartnerState.partner = null;
        lockBabySpawn = false;
    }
}
