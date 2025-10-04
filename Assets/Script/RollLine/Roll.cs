using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    //每个卡片之间差90距离

    [Header("释放卡片的位置")]
    public RectTransform Start_Position = null;

    [Header("第一个卡片位置")]
    public RectTransform First_Card_Position = null;

    [Header("卡片预制件")]
    public GameObject card = null;

    [Header("释放间隔时间")]
    public float interval = 6f;

    List<GameObject> cards = new List<GameObject>();

    void Start()
    {
        SpawnCard();
    }
    float timer = 0f;
    public void Update()
    {
        //最多9张卡片
        if (cards.Count >= 9)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;

            SpawnCard();
        }
    }

    public void SpawnCard()
    {
        //实例化新卡片
        GameObject newCard = Instantiate(card, Start_Position.position, Quaternion.identity, Start_Position.parent);        

        //添加到列表
        cards.Add(newCard);

        //设置卡片的roll属性
        Card c = newCard.GetComponent<Card>();
        c.roll = this;

        //随机一个建筑预制件
        int randomIndex = Random.Range(0, GlobalData.buildings.Count);
        c.buildingPrefab = GlobalData.buildings[randomIndex];
        c.SetSprite();

        //设置卡片的目标位置
        int index = cards.Count - 1;
        c.target = new Vector3(First_Card_Position.localPosition.x, First_Card_Position.localPosition.y - (index * 90), First_Card_Position.localPosition.z);
        c.StartRun();
    }

    public void UpdateCards()
    {
        //去除列表中为为空或未激活的卡片
        cards.RemoveAll(card => card == null || !card.activeInHierarchy);

        //卡片重新移动到正确位置
        for (int i = 0; i < cards.Count; i++)
        {
            Card c = cards[i].GetComponent<Card>();
            if (c != null)
            {
                c.target = new Vector3(First_Card_Position.localPosition.x, First_Card_Position.localPosition.y - (i * 90), First_Card_Position.localPosition.z);
                c.StartRun();
            }
        }

    }
}
