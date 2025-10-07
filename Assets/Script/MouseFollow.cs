using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseFollow : MonoBehaviour
{
    //维护子物体
    GameObject subObject = null;

    //维护当前卡片
    Card currentCard = null;

    Vector3 originalPosition;

    bool following = false;

    //是否可以点击
    bool canClick = true;

    Vector3 currentPos;

    //维护当前建筑的触发器列表
    List<TriggerCanPlace> triggerCanPlaces = new List<TriggerCanPlace>();

    //开始跟随鼠标
    public void StartFollow(Card card)
    {
        currentCard = card;
        GameObject targetPref = currentCard.buildingPrefab;
        //实例化预制件为子物体
        if (subObject == null && targetPref != null)
        {
            subObject = Instantiate(targetPref, transform);
            Collider2D[] colliders = subObject.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.isTrigger = true;
                triggerCanPlaces.Add(collider.gameObject.GetComponent<TriggerCanPlace>());//也可以用AddComponent
                if(collider.gameObject.GetComponent<Edge>() != null)
                {
                    Destroy(collider.gameObject.GetComponent<Edge>());
                }
            }
            subObject.transform.localPosition = Vector3.zero;

            currentCard.image.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            return;
        }

        following = true;
    }

    //停止跟随鼠标
    public void StopFollow()
    {
        //销毁子物体
        if (subObject != null)
        {
            currentCard.image.color = new Color(1, 1, 1, 1f);

            Destroy(subObject);
            subObject = null;
            currentCard = null;
        }

        following = false;
        transform.position = originalPosition;

        triggerCanPlaces.Clear();
    }

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        //右击取消
        if (following && Input.GetMouseButton(1))
        {
            StopFollow();
        }

        if (following)
        {
            FollowMousePosition();
        }

        //点击到UI时停止跟随
        if (following && Input.GetMouseButtonDown(0))
        {
            //用鼠标点击ui时才停止跟随
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                StopFollow(); 
            }
        }
        EventSystem.current.SetSelectedGameObject(null);

        UpdatePosition();
        
        
        canClick = SetCanClick();
        if (following && canClick && Input.GetMouseButton(0))
        {
            //执行功能
            Action(currentPos);

            StopFollow();
        }
        else if (following && !canClick && Input.GetMouseButton(0))
        {

            //无法放置的音效

        }
    }

    public bool SetCanClick()
    {
        for (int i = 0; i < triggerCanPlaces.Count; i++)
        {
            if (triggerCanPlaces[i] != null && !triggerCanPlaces[i].canPlace)
            {
                return false;
            }
        }
        return true;
    }

    public void Action(Vector3 pos)
    {
        if (currentCard != null)
        {
            currentCard.CardPlace(pos);
        }
    }

    //跟随方法主题
    private void FollowMousePosition()
    {
        //Vector3 mousePosition = Input.mousePosition;
        ////设置z坐标为物体到相机的距离，否则看不见
        //mousePosition.z = 4.5f;
        ////转换为世界坐标
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //worldPosition.z = originalPosition.z;
        ////更新物体位置
        //transform.position = worldPosition;

        transform.position = currentPos;
    }

    //更新位置：始终是鼠标位置最近的x.5, x.5的点
    public void UpdatePosition()
    {
        Vector3 mousePos = GetMouseWorldPosition2D();
        currentPos = new Vector3(Mathf.Round(mousePos.x - 0.5f) + 0.5f, Mathf.Round(mousePos.y - 0.5f) + 0.5f, 0);
    }

    //获取鼠标位置
    Vector3 GetMouseWorldPosition2D()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            return Vector3.zero;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z; // 对于2D正交相机
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0; // 确保Z坐标为0（2D）

        return worldPosition;
    }

}
