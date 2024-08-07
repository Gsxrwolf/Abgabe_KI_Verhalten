using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

public class CampfireBehavior : MonoBehaviour
{
    [SerializeField] private VisualEffect fireEffect;
    [SerializeField] private VisualEffect smomkeEffect;

    [SerializeField] private int killFireAfterRainInSec;
    [SerializeField] private int killSmokeAfterFireInSec;

    private bool isOff = false;

    public event Action startRain;
    void OnEnable()
    {
        smomkeEffect.enabled = true;
        fireEffect.SendEvent("StartFire");
        Clear.rainStarts += RainStarts;
    }

    private async void RainStarts()
    {
        if (isOff) return;

        smomkeEffect.SendEvent("StartSmoke");

        await Task.Delay(killFireAfterRainInSec * 1000);
        
        fireEffect.SendEvent("StopFire");

        await Task.Delay(killSmokeAfterFireInSec * 1000);

        smomkeEffect.SendEvent("StopSmoke");

        isOff= true;
    }


    private void OnDisable()
    {
        Clear.rainStarts -= RainStarts;
        smomkeEffect.enabled = false;
    }


}
