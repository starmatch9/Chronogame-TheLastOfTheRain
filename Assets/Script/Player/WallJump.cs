using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    Player player;

    //无法操纵角色协程
    Coroutine disableCoroutine = null;

    [Space]
    [Header("当前属性")]
    public float slideSpeed = 5;    //贴墙滑落速度
    public float disableTime = 0.1f;   //起跳后无法操纵的时长
    public float horizontalReduction = 1.44f; //水平速度缩小倍数
    public float verticalReduction = 1.44f;   //垂直速度缩小倍数
    public float wallJumpLerp = 5f;    //蹬墙跳插值，让空中停顿不生硬，模拟GetAxis惯性

    public void Awake()
    {
        player = GetComponent<Player>();
    }


    //墙上的跳跃滑落都归这个脚本管
    public void JumpOnWall(Vector2 dir)
    {
        //刚起跳的短暂时间内不能操纵角色
        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
            disableCoroutine = null;
        }
        disableCoroutine = StartCoroutine(DisableMovement(disableTime));

        //判断方向，在右边墙向左跳跃，反之亦然
        Vector2 wallDir = dir;

        //防止滑动速度影响，重置y轴速度
        player.rb.velocity = new Vector2(player.rb.velocity.x, 0);

        //斜上方跳跃，高度比地面跳跃低
        player.JumpScript.BasicJump((Vector2.up / verticalReduction + wallDir / horizontalReduction));
    }

    public void SlideOnWall()
    {
        //有效阻止跳跃时的滑落
        if (!player.canMove)
        {
            return;
        }

        player.rb.velocity = new Vector2(player.rb.velocity.x, -slideSpeed);
    }

    //无法操纵协程
    IEnumerator DisableMovement(float time)
    {
        player.canMove = false;
        yield return new WaitForSeconds(time);
        player.canMove = true;
    }
}
