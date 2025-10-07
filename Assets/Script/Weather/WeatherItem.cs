using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//序列化，检查其可见
[System.Serializable]
public class WeatherItem
{
    //天气名称
    public string name; 

    //天气图标
    public Sprite icon;
    
    //每单位降雨量
    public int rainPerUnit;

    //海面速度
    public float seaSpeed;

    //随机出现权重
    public int weight;

    //持续时间
    public float duration;

    //持续时间波动范围
    public float durationRange;

    //对应的背景音乐
    public AudioClip voice;
}
