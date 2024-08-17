using System.Collections;
using UnityEngine;

public class WolfAttackState : WolfBaseState
{
    private WolfStateMachine context;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDuration;

    private bool isAttacking;
    public override void Enter(WolfStateMachine _context)
    {
        _context.anim.SetTrigger("Attack");
        context = _context;

        context.agent.speed = 0.01f;

        isAttacking = true;
    }
    public override void Do(WolfStateMachine _context)
    {
    }
    public void StartJumpAttack()
    {
    }
    public void CheckHit()
    {
        GameObject target = context.GetNearestSheep(transform.position);

        if (Vector3.Distance(transform.position, target.transform.position) < 5f)
        {
            target.GetComponent<SheepStateMachine>().DealDamage();
        }
    }

    private void EndJumpAttack()
    {
        StartCoroutine(MoveForward());
        isAttacking = false;
        context.anim.ResetTrigger("Attack");
    }
    private IEnumerator MoveForward()
    {
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition - transform.forward * moveSpeed * moveDuration;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
    public override void FixedDo(WolfStateMachine _context)
    {
    }
    public override void CheckState(WolfStateMachine _context)
    {
        if (!isAttacking)
        {
            _context.SwitchState(_context.wolfidleState);
        }
    }
    public override void Exit(WolfStateMachine _context)
    {
    }
}
