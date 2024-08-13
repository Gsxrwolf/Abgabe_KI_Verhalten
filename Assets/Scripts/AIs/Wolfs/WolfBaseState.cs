using UnityEngine;

public abstract class WolfBaseState : MonoBehaviour
{
    public abstract void Enter(WolfStateMachine _context);
    public abstract void Do(WolfStateMachine _context);
    public abstract void FixedDo(WolfStateMachine _context);
    public abstract void CheckState(WolfStateMachine _context);
    public abstract void Exit(WolfStateMachine _context);
}