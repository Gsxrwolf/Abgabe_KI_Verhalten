using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyRain : WeatherBaseState
{
    private float timeUntillLessRain;

    private float timer = 0;

    private bool rainPossible = false;

    private WeatherStateMachine context;
    public override void Enter(WeatherStateMachine _context)
    {
        context = _context;
        Debug.Log("Clear");
        _context.rainParticalSystem.gameObject.SetActive(false);

        timeUntillLessRain = GetRandomTimeToRain();
    }
    private float GetRandomTimeToRain()
    {
        float time = 0;
        System.Random rnd = new System.Random();

        time = rnd.Next(context.timeSwitchMin, context.timeSwitchMax);

        return time;
    }
    public override void Do(WeatherStateMachine _context)
    {
        if (timer < timeUntillLessRain)
        {
            timer += Time.deltaTime;
            return;
        }
        timer = 0;
        rainPossible = true;
    }
    public override void CheckState(WeatherStateMachine _context)
    {
        if (rainPossible)
        {
            System.Random rnd = new System.Random();
            int temp = rnd.Next(100);
            if (temp < 10)
            {
                _context.SwitchState(_context.clearState);
            }
            else if (temp > 10 && temp < 30)
            {
                _context.SwitchState(_context.lightRainState);
            }
            else if (temp > 30)
            {
                _context.SwitchState(_context.mediumRainState);
            }
        }
    }
    public override void FixedDo(WeatherStateMachine _context)
    {

    }
    public override void Exit(WeatherStateMachine _context)
    {

    }
}
