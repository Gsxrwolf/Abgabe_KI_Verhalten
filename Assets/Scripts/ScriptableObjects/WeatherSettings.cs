using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeatherValues", menuName = "tools/WeatherValues")]
public class WeatherSettings : ScriptableObject
{
    [SerializeField] public int clearEmissionRate = 0;
    [SerializeField] public int lightRainEmissionRate = 250;
    [SerializeField] public int mediumRainEmissionRate = 2000;
    [SerializeField] public int heavyRainEmissionRate = 20000;


    [SerializeField] public float clearGrassSpeed = 0.1f;
    [SerializeField] public float lightRainGrassSpeed = 0.4f;
    [SerializeField] public float mediumRainGrassSpeed = 0.8f;
    [SerializeField] public float heavyRainGrassSpeed = 1f;

    [SerializeField] public Color clearGrassColor;
    [SerializeField] public Color lightRainGrassColor;
    [SerializeField] public Color mediumRainGrassColor;
    [SerializeField] public Color heavyRainGrassColor;
}
