using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QualitySettings", menuName = "tools/qualitySettings")]
public class OwnQualitySettings : ScriptableObject
{
    [SerializeField] public bool[] rainParticalEffectIsEnabled = new bool[3];
    [SerializeField] public bool[] splashParticalEffectIsEnabled = new bool[3];
    [SerializeField,Range(0,1)] public float[] weathSettingsMultiplyer = new float[3];

    [SerializeField] public bool[] sheepPrefabFurGeneratorIsEnabled = new bool[3];
    [SerializeField] public bool[] dogFurGeneratorIsEnabled = new bool[3];

    [SerializeField,Range(0,1)] public float[] terrainDetailResolution = new float[3];
}
