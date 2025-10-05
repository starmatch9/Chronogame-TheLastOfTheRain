using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    //用于调整碰撞体

    //此脚本存在的缺陷（目前测试出的）：
    //1、不可以重叠两个碰撞器（计数器）
    //2、不适用于非方形碰撞体
    //3、与输入检测不同步（目前微微解决）

    [Header("可接触地形的对应标签")]
    public string groundTag = "Ground";

    [Space]
    public bool onGround;
    public bool onRightWall;
    public bool onLeftWall;

    //杜绝多重碰撞体的影响！！！
    int groundContactCount = 0;
    int leftWallContactCount = 0;
    int rightWallContactCount = 0;

    //利用接触点，存放法线方向和对应的碰撞体
    Dictionary<GameObject , Vector2> keyValuePairs = new Dictionary<GameObject, Vector2>();

    void OnCollisionEnter2D(Collision2D collision)
    {
        //检测指定标签的地形
        if (collision.gameObject.CompareTag(groundTag))
        {
            //获取第一个接触点
            ContactPoint2D contact = collision.contacts[0];

            //获取法线方向
            Vector2 hitDirection = contact.normal;

            //记录当前碰撞体的接触方向
            if (hitDirection.y > 0.5f)
            {
                keyValuePairs[collision.gameObject] = hitDirection;
                groundContactCount++;

                onGround = groundContactCount > 0;
            }
            else if (hitDirection.x > 0.5f)
            {
                keyValuePairs[collision.gameObject] = hitDirection;
                rightWallContactCount++;

                onRightWall = rightWallContactCount > 0;
            }
            else if (hitDirection.x < -0.5f)
            {
                keyValuePairs[collision.gameObject] = hitDirection;
                leftWallContactCount++;

                onLeftWall = leftWallContactCount > 0;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            //如果没有记录该碰撞体，则直接返回
            if (!keyValuePairs.ContainsKey(collision.gameObject))
            {
                return;
            }

            Vector2 hitDirection = keyValuePairs[collision.gameObject];

            if (hitDirection.y > 0.5f)
            {
                groundContactCount = Mathf.Max(0, groundContactCount - 1); //确保不会变成负数

                onGround = groundContactCount > 0;
            }
            else if (hitDirection.x > 0.5f)
            {
                rightWallContactCount = Mathf.Max(0, rightWallContactCount - 1);

                onRightWall = rightWallContactCount > 0;
            }
            else if (hitDirection.x < -0.5f)
            {
                leftWallContactCount = Mathf.Max(0, leftWallContactCount - 1);

                onLeftWall = leftWallContactCount > 0;
            }
        }
    }
}
