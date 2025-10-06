using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    public static MonoBehaviour mono;

    public static Player player;

    public static List<GameObject> buildings = new List<GameObject>();

    public static Transform buildingParent;

    public static MouseFollow mouseFollow;

    public static Sea sea;

    public static Rain rain;

    public static HealthBar healthBar;

    public static Recard recard;

    public static Recard_UI recard_UI;

    public static StopGame stopGame;

    //人物血量的最大值
    public static int playerHealth;
    public static int currentHealth;

    //天气管理(好像这种直接用管理器的方法也不错)
    public static WeatherManager weatherManager;


    /*游戏结束逻辑*/
    public static void EndGame()
    {
        stopGame.ShowScore(recard.highestY);
    }


    /*血量逻辑*/
    //受伤后3秒内如果没有再次受伤则恢复所有血量
    static Coroutine hurtCoroutine = null;
    public static void hurt()
    {
        --currentHealth;

        healthBar.SetHealth((float)currentHealth / playerHealth);

        if(currentHealth <= 0)
        {
            //游戏结束
            EndGame();

            Debug.Log("Game Over!");
            return;
        }

        if (hurtCoroutine != null)
        {
            mono.StopCoroutine(hurtCoroutine);
            hurtCoroutine = null;
        }
        hurtCoroutine = mono.StartCoroutine(hurtTimer());
    }
    public static IEnumerator hurtTimer()
    {
        //受伤后3秒内如果没有再次受伤则恢复所有血量
        yield return new WaitForSeconds(2f);

        currentHealth = playerHealth;

        //血量条插值变化

        float timer = 0f;
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            healthBar.SetHealth(Mathf.Lerp(healthBar.fillImage.fillAmount, 1f, timer));
            yield return null;
        }

        hurtCoroutine = null;
    }




}
