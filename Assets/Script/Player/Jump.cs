using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    Player player;

    //无法跳跃角色协程
    Coroutine disableCoroutine = null;

    [TextArea]
    public string description = "按键按住的速度需要通过刚体的重力大小与跳跃速度共同决定调节。";

    [Space]
    [Header("当前属性")]
    public float jumpForce = 30f;   //跳跃速度
    public float disableTime = 0.08f;   //起跳后无法再次起跳的时长

    [Space]
    [Header("上升与下落的重力加速度倍数")]
    public float jumpGravityScale = 8f;
    public float fallGravityScale = 6f;

    public void Awake()
    {
        player = GetComponent<Player>();
    }

    public void Update()
    {
        //每帧调整重力
        if (player.rb.velocity.y < 0)
        {
            //下落时增加重力
            player.rb.velocity += Vector2.up * Physics2D.gravity.y * (fallGravityScale - 1) * Time.deltaTime;

            //速度最大只能是30
            if (player.rb.velocity.y < -25f)
            {
                player.rb.velocity = new Vector2(player.rb.velocity.x, -25f);
            }
        }
        else if (player.rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            //上升时，如果按键没松开就不增加重力，普通下落
            player.rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpGravityScale - 1) * Time.deltaTime;
        }
    }

    //基础跳跃：提供一个方向，进行跳跃
    public void BasicJump(Vector2 dir)
    {
        if(!player.canJump)
        {
            return;
        }

        GlobalData.effectManager.PlayEffect(GlobalData.effectManager.jump);

        //提供初速度
        player.rb.velocity = new Vector2(player.rb.velocity.x, 0);
        //物体原速度加上起跳速度
        player.rb.velocity += dir * jumpForce;

        //刚起跳的短暂时间内不能操纵角色
        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
            disableCoroutine = null;
        }
        disableCoroutine = StartCoroutine(DisableJump(disableTime));

        player.jumped = true;
    }

    //无法操纵协程
    IEnumerator DisableJump(float time)
    {
        player.canJump = false;
        yield return new WaitForSeconds(time);
        player.canJump = true;
    }
}
