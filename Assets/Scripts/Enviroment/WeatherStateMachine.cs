using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Rendering.InspectorCurveEditor;

public class WeatherStateMachine : MonoBehaviour
{
    public Clear clearState;
    public LightRain lightRainState;
    public MediumRain mediumRainState;
    public HeavyRain heavyRainState;
    private WeatherBaseState curState;

    [SerializeField] public int lightRainEmissionRate;
    [SerializeField] public int mediumRainEmissionRate;
    [SerializeField] public int heavyRainEmissionRate;


    [SerializeField] public int timeSwitchMin;
    [SerializeField] public int timeSwitchMax;


    [SerializeField] public ParticleSystem rainParticalSystem;
    public ParticleSystem.EmissionModule emission;
    void Start()
    {
        emission = rainParticalSystem.emission;
    }
    void OnEnable()
    {
        curState = clearState;
        curState.Enter(this);
    }

    private void Update()
    {
        curState.Do(this);
        curState.CheckState(this);
    }
    public void SwitchState(WeatherBaseState _newState)
    {
        curState.Exit(this);
        curState = _newState;
        curState.Enter(this);
    }
}