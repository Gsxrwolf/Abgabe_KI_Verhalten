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

    void OnEnable()
    {
        smomkeEffect.enabled = true;
        fireEffect.SendEvent("StartFire");
        Clear.rainStarts += RainStarts;
    }

    private async void RainStarts()
    {

        if (smomkeEffect != null) smomkeEffect.SendEvent("StartSmoke");

        await Task.Delay(killFireAfterRainInSec * 1000);

        if (fireEffect != null) fireEffect.SendEvent("StopFire");

        await Task.Delay(killSmokeAfterFireInSec * 1000);

        if(smomkeEffect != null) smomkeEffect.SendEvent("StopSmoke");

        OnDisable();
    }


    private void OnDisable()
    {
        Clear.rainStarts -= RainStarts;
        if (smomkeEffect != null) smomkeEffect.enabled = false;
    }


}
