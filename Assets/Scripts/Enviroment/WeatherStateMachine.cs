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

    public WeatherSettings weatherSettings;

    [SerializeField] public int timeSwitchMin;
    [SerializeField] public int timeSwitchMax;

    [HideInInspector] public float lerpTimer;
    public float lerpDuration;


    [SerializeField] public ParticleSystem rainParticalSystem;
    [HideInInspector] public ParticleSystem.EmissionModule emission;

    [HideInInspector] public TerrainData terrainData;
    void Start()
    {
        emission = rainParticalSystem.emission;
        terrainData = GetComponent<Terrain>().terrainData;
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