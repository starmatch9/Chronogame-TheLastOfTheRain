using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("玩家")]
    public Player player;

    [Header("水平面游戏对象")]
    public Sea sea;

    [Header("降雨游戏对象")]
    public Rain rain;

    [Header("血量条对象")]
    public HealthBar healthBar;

    [Header("人物高度记录对象")]
    public Recard recard;

    [Header("暂停游戏对象")]
    public StopGame stopGame;

    [Header("人物血量")]
    public int playerHealth = 150;

    private void Awake()
    {
        GlobalData.mono = this;

        GlobalData.sea = sea;

        GlobalData.rain = rain;

        GlobalData.healthBar = healthBar;

        GlobalData.recard = recard;

        GlobalData.player = player;

        GlobalData.stopGame = stopGame;

        GlobalData.playerHealth = playerHealth;
        GlobalData.currentHealth = playerHealth;
    }

    public void Start()
    {
        sea.StartRising();
    }
}
