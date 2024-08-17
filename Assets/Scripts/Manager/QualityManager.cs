using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class QualityManager : MonoBehaviour
{
    private int curQualitySetting;
    [SerializeField] private OwnQualitySettings settings;

    [SerializeField] private WeatherSettings weatherSettings;
    [SerializeField] private GameObject rainParticalEffect;
    [SerializeField] private GameObject splashParticalEffect;

    [SerializeField] private PoolSpawner sheepSpawner;
    [SerializeField] private FurGenerator dogFurGenerator;

    [SerializeField] private Terrain terrainMap;

    [SerializeField] private UniversalRenderPipelineAsset lowUniversalRenderPipelineAsset;
    [SerializeField] private UniversalRenderPipelineAsset mediumUniversalRenderPipelineAsset;
    [SerializeField] private UniversalRenderPipelineAsset highUniversalRenderPipelineAsset;


    private int clearWeatherCache;
    private int lightWeatherCache;
    private int mediumWeatherCache;
    private int heavyWeatherCache;


    private float renderScaleCache;




    void Awake()
    {
        curQualitySetting = GameManager.Instance.saveFile.qualitySetting;

        ApplyQualitySettings(curQualitySetting);
    }

    private void ApplyQualitySettings(int qualitySetting)
    {
        rainParticalEffect.SetActive(settings.rainParticalEffectIsEnabled[qualitySetting]);
        splashParticalEffect.SetActive(settings.splashParticalEffectIsEnabled[qualitySetting]);

        sheepSpawner.disableFur = !settings.sheepPrefabFurGeneratorIsEnabled[qualitySetting];
        dogFurGenerator.disableFur = !settings.dogFurGeneratorIsEnabled[qualitySetting];

        terrainMap.detailObjectDensity = settings.terrainDetailResolution[qualitySetting];
        WalkingPathTrail.normalGrassHeight = (int)(255 * settings.terrainDetailResolution[qualitySetting]);

        clearWeatherCache = weatherSettings.clearEmissionRate;
        lightWeatherCache = weatherSettings.lightRainEmissionRate;
        mediumWeatherCache = weatherSettings.mediumRainEmissionRate;
        heavyWeatherCache = weatherSettings.heavyRainEmissionRate;

        weatherSettings.clearEmissionRate = (int)(clearWeatherCache * settings.weathSettingsMultiplyer[qualitySetting]);
        weatherSettings.lightRainEmissionRate = (int)(lightWeatherCache * settings.weathSettingsMultiplyer[qualitySetting]);
        weatherSettings.mediumRainEmissionRate = (int)(mediumWeatherCache * settings.weathSettingsMultiplyer[qualitySetting]);
        weatherSettings.heavyRainEmissionRate = (int)(heavyWeatherCache * settings.weathSettingsMultiplyer[qualitySetting]);

        switch (qualitySetting)
        {
            case 0:
                QualitySettings.SetQualityLevel(0); 
                GraphicsSettings.renderPipelineAsset = lowUniversalRenderPipelineAsset;
                break;
            case 1:
                QualitySettings.SetQualityLevel(1);
                GraphicsSettings.renderPipelineAsset = mediumUniversalRenderPipelineAsset;
                break;
            case 2:
                QualitySettings.SetQualityLevel(2);
                GraphicsSettings.renderPipelineAsset = highUniversalRenderPipelineAsset;
                break;
        }
    }
    private void OnDestroy()
    {
        weatherSettings.clearEmissionRate = clearWeatherCache;
        weatherSettings.lightRainEmissionRate = lightWeatherCache;
        weatherSettings.mediumRainEmissionRate = mediumWeatherCache;
        weatherSettings.heavyRainEmissionRate = heavyWeatherCache;
    }
}