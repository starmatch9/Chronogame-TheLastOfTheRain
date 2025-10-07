using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [HideInInspector]
    public Roll roll = null;

    [HideInInspector]
    public Vector3 target;

    [Header("移动速度")]
    public float speed = 10f;

    [Header("缩略图游戏对象")]
    public Image image;

    [Header("对应建筑预制件")]
    public GameObject buildingPrefab;

    bool run = false;

    public void OnCardClick()
    {
        if (roll == null || buildingPrefab == null)
        {
            return;
        }

        GlobalData.effectManager.PlayEffect(GlobalData.effectManager.UI);

        //告诉鼠标跟随，开始跟随这个建筑物预制件
        if (GlobalData.mouseFollow != null)
        {
            GlobalData.mouseFollow.StartFollow(this);
        }
    }

    public void CardPlace(Vector3 pos)
    {
        GlobalData.effectManager.PlayEffect(GlobalData.effectManager.UI);

        //在pos放置建筑
        if (buildingPrefab != null)
        {
            Instantiate(buildingPrefab, pos, Quaternion.identity, GlobalData.buildingParent);
        }

        //销毁
        gameObject.SetActive(false);
        roll.UpdateCards();
        Destroy(gameObject);
    }


    public void SetSprite()
    {
        if (buildingPrefab == null)
        {
            return;
        }
        BuildingInformation info = buildingPrefab.GetComponent<BuildingInformation>();
        if (info == null)
        {
            return;
        }
        image.sprite = info.Sprite;
    }

    public void StartRun()
    {
        run = true;
    }

    private void Update()
    {
        if (!run)
        {
            return;
        }

        //向上移动
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        //如果超出第一个卡片位置，停止移动，并且对齐位置
        if (transform.localPosition.y >= target.y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, target.y, transform.localPosition.z);
            run = false;
        }
    }
}
