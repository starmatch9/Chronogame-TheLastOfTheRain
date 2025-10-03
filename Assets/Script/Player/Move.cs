using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    Player player;

    [TextArea]
    public string description = "移动加速减速的力度需要在输入管理器中调节。";

    [Space]
    [Header("当前属性")]
    public float speed = 10;    //移动速度

    public void Awake()
    {
        player = GetComponent<Player>();
    }

    //传入一个方向，进行移动
    public void Walk(Vector2 dir)
    {
        //角色不可操控时，直接返回
        if (!player.canMove)
        {
            return;
        }

        player.rb.velocity = new Vector2(dir.x * speed, player.rb.velocity.y);
    }
}
