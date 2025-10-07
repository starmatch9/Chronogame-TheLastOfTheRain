using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherManager : MonoBehaviour
{
    [Header("天气音效管理器")]
    public VoiceManger voiceManger;

    [Header("填充对象")]
    public Image weatherFill;

    [Header("图标对象")]
    public Image weatherIcon;

    [Header("天气列表")]
    public List<WeatherItem> weatherItems;

    [Space]
    [Header("每多少米各个权重增加多少")]
    [Header("列表索引与天气索引一一对应")]
    public int meter = 100;
    public List<int> weightIncrease = new List<int>();


    //当前要设置的天气的索引
    int currentWeatherIndex = 0;

    Coroutine exist = null;

    private void Awake()
    {
        GlobalData.weatherManager = this;
        currentWeatherIndex = 0; //开始时是晴天
    }

    //开始时必须是晴天
    private void Start()
    {
        SetWeather(currentWeatherIndex);
    }

    //每隔多少高度生成一次技能
    int number = 0;
    void Update()
    {
        if (GlobalData.recard.highestY % meter == 0 && GlobalData.recard.highestY / meter > number)
        {
            //增加各个天气的权重
            for (int i = 0; i < weatherItems.Count && i < weightIncrease.Count; i++)
            {
                weatherItems[i].weight += weightIncrease[i];
            }
            number++;
        }
    }

    //根据索引设置当前天气！
    public void SetWeather(int index)
    {
        if (index < 0 || index >= weatherItems.Count)
        {
            return;
        }
        //设置天气
        WeatherItem selectedWeather = weatherItems[index];
        Debug.Log("设置天气为" + selectedWeather.name);

        //设置持续时间、范围波动、下一个天气’降雨量、海面速度
        GlobalData.sea.speed = selectedWeather.seaSpeed;
        GlobalData.rain.SetRainIntensity(selectedWeather.rainPerUnit);

        if (exist != null)
        {
            StopCoroutine(exist);
            exist = null;
        }
        exist = StartCoroutine(Exist(selectedWeather.duration + Random.Range(-selectedWeather.durationRange, selectedWeather.durationRange), selectedWeather));

        //获取下一个天气索引
        currentWeatherIndex = GetWeightedRandom(index);
        weatherIcon.sprite = weatherItems[currentWeatherIndex].icon;
    }



    IEnumerator Exist(float duration, WeatherItem selectedWeather)
    {
        //yield return new WaitForSeconds(duration);

        voiceManger.PlayVoice(selectedWeather.voice);

        //填充从满到空
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            weatherFill.fillAmount = Mathf.Lerp(1f, 0f, timer / duration);
            yield return null;
        }

        voiceManger.StopVoice();

        //时间到，切换到下一个天气
        SetWeather(currentWeatherIndex);
    }

    //根据权重随机选择下一个天气索引（且能跳过当前索引）
    public int GetWeightedRandom(int jumpIndex)
    {
        // 定义权重
        List<int> weights = new List<int>();

        for (int i = 0; i < weatherItems.Count; i++)
        {
            if (i == jumpIndex)
            {
                weights.Add(0); //跳过当前索引，就是权重为0
            }
            else
            {
                weights.Add(weatherItems[i].weight);
            }
        }   

        // 计算总权重
        int totalWeight = 0;
        foreach (int weight in weights)
        {
            totalWeight += weight;
        }

        // 生成随机数
        int randomValue = Random.Range(0, totalWeight);

        int currentWeight = 0;

        // 根据权重选择结果
        for (int i = 0; i < weights.Count; i++)
        {
            currentWeight += weights[i];
            if (randomValue < currentWeight)
            {
                //返回选择的索引
                return i;
            }
        }

        return 0; // 默认返回0
    }

}
