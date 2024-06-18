using UnityEngine;

public class MediumRain : WeatherBaseState
{
    private float timeUntillSwitch;

    private float timer = 0;

    private bool rainGettingStronger = false;
    private bool switchReady;

    private WeatherStateMachine context;
    public override void Enter(WeatherStateMachine _context)
    {
        context = _context;
        Debug.Log("Medium");
        _context.emission.rateOverTime = _context.lightRainEmissionRate;

        rainGettingStronger = GetRandomTrueOrFalse();
        timeUntillSwitch = GetRandomTimeToSwitch();
    }
    private bool GetRandomTrueOrFalse()
    {
        System.Random rnd = new System.Random();
        int temp = rnd.Next(2);
        if (temp == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private float GetRandomTimeToSwitch()
    {
        float time = 0;
        System.Random rnd = new System.Random();

        time = rnd.Next(context.timeSwitchMin, context.timeSwitchMax);

        return time;
    }
    public override void Do(WeatherStateMachine _context)
    {
        if (timer < timeUntillSwitch)
        {
            timer += Time.deltaTime;
            return;
        }
        timer = 0;
        switchReady = true;
    }
    public override void CheckState(WeatherStateMachine _context)
    {
        if (switchReady)
        {
            if (rainGettingStronger)
            {
                _context.SwitchState(_context.heavyRainState);
            }
            else
            {
                _context.SwitchState(_context.lightRainState);
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
