using System;
using UnityEngine;

public class Clear : WeatherBaseState
{
    private float timeUntillRain;

    private float timer = 0;

    private bool rainPossible = false;

    private WeatherStateMachine context;
    public override void Enter(WeatherStateMachine _context)
    {
        context = _context;
        Debug.Log("Clear");
        _context.rainParticalSystem.gameObject.SetActive(false);

        timeUntillRain = GetRandomTimeToRain();
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
        if (timer < timeUntillRain)
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
            int temp = rnd.Next(2);
            if(temp == 0)
            {
                _context.SwitchState(_context.lightRainState);
            }
            else
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
        _context.rainParticalSystem.gameObject.SetActive(true);
    }
}
