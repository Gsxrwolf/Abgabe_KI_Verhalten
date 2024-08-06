using UnityEngine;

public class HeavyRain : WeatherBaseState
{
    private float timeUntillLessRain;

    private float timer = 0;

    private bool switchReady = false;

    private WeatherStateMachine context;
    public override void Enter(WeatherStateMachine _context)
    {
        context = _context;

        timeUntillLessRain = GetRandomTimeToRainLess();
    }
    private float GetRandomTimeToRainLess()
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

            _context.emission.rateOverTime = Mathf.Lerp(_context.emission.rateOverTime.constant, _context.weatherSettings.heavyRainEmissionRate, lerpFactor);
            _context.terrainData.wavingGrassStrength = Mathf.Lerp(_context.terrainData.wavingGrassStrength, _context.weatherSettings.heavyRainGrassSpeed, lerpFactor);
            _context.terrainData.wavingGrassTint = Color.Lerp(_context.terrainData.wavingGrassTint, _context.weatherSettings.heavyRainGrassColor, lerpFactor);
        }


        if (timer < timeUntillLessRain)
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
        timeUntillLessRain = 0;

        timer = 0;

        switchReady = false;

        _context.lerpTimer = 0;
    }
}