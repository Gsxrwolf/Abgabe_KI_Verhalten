using System;
using System.Threading;
using UnityEngine;

public abstract class SheepBaseState : MonoBehaviour
{
    public abstract void Enter(SheepStateMachine _context);
    public abstract void Do(SheepStateMachine _context);
    public abstract void FixedDo(SheepStateMachine _context);
    public abstract void CheckState(SheepStateMachine _context);
    public abstract void Exit(SheepStateMachine _context);
}