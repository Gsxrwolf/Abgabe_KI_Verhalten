using System;
using UnityEngine;

public class Clear : WeatherBaseState
{
    private float timeUntillRain;

    private float timer = 0;

    private bool rainPossible = false;

    private WeatherStateMachine context;

    public static event Action rainStarts;

    public override void Enter(WeatherStateMachine _context)
    {
        context = _context;

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
        if (_context.lerpTimer < _context.lerpDuration)
        {
            _context.lerpTimer += Time.deltaTime;
            float lerpFactor = _context.lerpTimer / _context.lerpDuration;

            _context.emission.rateOverTime = Mathf.Lerp(_context.emission.rateOverTime.constant, _context.weatherSettings.clearEmissionRate, lerpFactor);
            _context.terrainData.wavingGrassStrength = Mathf.Lerp(_context.terrainData.wavingGrassStrength, _context.weatherSettings.clearGrassSpeed, lerpFactor);
            _context.terrainData.wavingGrassTint = Color.Lerp(_context.terrainData.wavingGrassTint, _context.weatherSettings.clearGrassColor, lerpFactor);
        }
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
                rainStarts?.Invoke();
                _context.SwitchState(_context.lightRainState);
            }
            else
            {
                rainStarts?.Invoke();
                _context.SwitchState(_context.mediumRainState);
            }
        }
    }
    public override void FixedDo(WeatherStateMachine _context)
    {

    }
    public override void Exit(WeatherStateMachine _context)
    {
        timer = 0;
        timeUntillRain = 0;
        rainPossible = false;

        _context.lerpTimer = 0;
    }
}
