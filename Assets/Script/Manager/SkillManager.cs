using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;       

public class SkillManager : MonoBehaviour
{
    [Header("生成位置父物体")]
    public Transform spawnPointParent;

    [Header("技能预制体")]
    public List<GameObject> skillPrefabs;

    [Header("每多少高度生成一次技能")]
    public float spawnHeight = 5f;

    [Header("伞图标与二段跳图标")]
    public Sprite umbrellaIcon;
    public Sprite jumpTwoIcon;

    [Header("伞与二段跳持续时间")]
    public float umbrellaDuration = 5f;
    public float jumpTwoDuration = 5f;

    [Header("技能UI")]
    public GameObject skillUI;

    [Header("UI图标对象")]
    public Image image;

    [Header("填充游戏对象")]
    public Image fill;

    [Space]
    [Header("伞区域")]
    [Header("Umb预制件")]
    public GameObject umb = null;



    [Space]
    [Header("二段跳区域")]

    int number = 0;
    void Update()
    {
        if(GlobalData.recard.highestY % spawnHeight == 0 && GlobalData.recard.highestY / spawnHeight > number)
        {
            SpawnSkill();
            number++;
        }
    }

    void SpawnSkill()
    {
        if (skillPrefabs.Count == 0) return;
        //随机选择一个技能预制体
        GameObject skillPrefab = skillPrefabs[Random.Range(0, skillPrefabs.Count)];

        Skill skill = skillPrefab.GetComponent<Skill>();
        skill.skillManager = this;

        //高度为摄像机世界位置的y值+20.x轴位置为(-15, 15)范围内的随机值.z轴位置为0
        Vector3 spawnPosition = new Vector3(Random.Range(-15f, 15f), Camera.main.transform.position.y + 20f, 0f);

        //实例化技能预制体
        Instantiate(skillPrefab, spawnPosition, Quaternion.identity, spawnPointParent);
    }

    //维护雨伞实例
    GameObject umbrellaInstance = null;

    //技能计时逻辑
    Coroutine skill = null;
    //技能开始计时
    //
    //一个技能一个编号：雨伞是0，二段跳是1
    public void SkillTime(int index)
    {
        if (skill != null)
        {
            if(GlobalData.player.maxJumpCount == 2)
            {
                GlobalData.player.maxJumpCount = 1;
            }

            if(umbrellaInstance != null)
            {
                Destroy(umbrellaInstance);
                umbrellaInstance = null;
            }

            StopCoroutine(skill);
            skill = null;
        }

        if(index == 0)
        {
            skill = StartCoroutine(UmbrellaSkillTimer(umbrellaDuration));
        }
        else
        {
            skill = StartCoroutine(JumpTwoSkillTimer(jumpTwoDuration));
        }
        
    }


    IEnumerator UmbrellaSkillTimer(float duration)
    {
        Debug.Log("开启雨伞技能");
        StartCoroutine(JumpIn());

        umbrellaInstance = Instantiate(umb, new Vector3(0, 0, 0), Quaternion.identity);

        //技能持续时间
        //yield return new WaitForSeconds(duration);
        image.sprite = umbrellaIcon;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            fill.fillAmount = 1 - timer / duration;
            yield return null;
        }

        Destroy(umbrellaInstance);
        umbrellaInstance = null;

        StartCoroutine(JumpOut());
        skill = null;
    }

    IEnumerator JumpTwoSkillTimer(float duration)
    {
        Debug.Log("开启二段跳技能");
        StartCoroutine(JumpIn());

        GlobalData.player.maxJumpCount = 2;

        //技能持续时间
        //yield return new WaitForSeconds(duration);
        image.sprite = jumpTwoIcon;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            fill.fillAmount = 1 - timer / duration;
            yield return null;
        }

        GlobalData.player.maxJumpCount = 1;

        StartCoroutine(JumpOut());
        skill = null;
    }

    //弹窗协程
    public IEnumerator JumpIn()
    {
        //修改局部坐标而非世界坐标
        float timer = 0;
        //一秒钟如何
        while (timer < 0.5f)
        {
            //从0到500插值
            float x = Mathf.Lerp(1100, 650, timer / 0.5f);
            Vector3 v = new Vector3(x, -335, 0);
            skillUI.transform.localPosition = v;

            timer += Time.deltaTime;
            yield return null;
        }
        skillUI.transform.localPosition = new Vector3(650, -335, 0);
    } 
    public IEnumerator JumpOut()
    {
        //修改局部坐标而非世界坐标
        float timer = 0;
        //一秒钟如何
        while (timer < 0.5f)
        {
            //从0到500插值
            float x = Mathf.Lerp(650, 1100, timer / 0.5f);
            Vector3 v = new Vector3(x, -335, 0);
            skillUI.transform.localPosition = v;

            timer += Time.deltaTime;
            yield return null;
        }
        skillUI.transform.localPosition = new Vector3(1100, -335, 0);
    }
}
