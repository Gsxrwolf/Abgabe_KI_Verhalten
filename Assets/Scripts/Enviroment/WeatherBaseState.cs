using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeatherBaseState : MonoBehaviour
{
    public abstract void Enter(WeatherStateMachine _context);
    public abstract void Do(WeatherStateMachine _context);
    public abstract void FixedDo(WeatherStateMachine _context);
    public abstract void CheckState(WeatherStateMachine _context);
    public abstract void Exit(WeatherStateMachine _context);
}
