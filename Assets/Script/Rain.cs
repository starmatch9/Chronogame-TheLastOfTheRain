using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    public ParticleSystem rainParticleSystem;


    //开始或结束下雨
    public void StartRaining()
    {
        if (rainParticleSystem != null && !rainParticleSystem.isPlaying)
        {
            rainParticleSystem.Play();
        }
    }
    public void StopRaining()
    {
        if (rainParticleSystem != null && rainParticleSystem.isPlaying)
        {
            rainParticleSystem.Stop();
        }
    }
    //设置雨量大小
    public void SetRainIntensity(float number)
    {
        if (rainParticleSystem != null)
        {
            var emission = rainParticleSystem.emission;
            emission.rateOverTime = number;
        }
    }

}
