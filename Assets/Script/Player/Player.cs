using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //角色编辑器：启用某项能力，作为单例存在

    //刚体组件：用来运动的核心
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Collision coll;

    //启用各个能力脚本
    [Space]
    [Header("启用各个能力")]
    public bool Move = true;
    public bool Jump = true;
    public bool WallJump = true;

    //经过不断调试，推荐跳跃缓冲时间为土狼时间的一半，跳跃无效时长与土狼一致
    [Space]
    [Header("土狼时间与跳跃缓冲")]
    public float coyoteTime = 0.08f;
    public float bufferTime = 0.04f;

    //上面两个的计时器
    public float coyoteTimer = 0;
    public float coyoteTimerLeft = 0;
    public float coyoteTimerRight = 0;
    public float bufferTimer = 0;

    //记录当前状态
    [Space]
    [Header("当前状态")]
    public bool canMove = true;   //玩家是否可左右操纵角色
    public bool canJump = true;   //玩家是否可跳跃
    public bool wallSlide = false;    //是否处于滑落状态

    [Space]
    [Header("最大跳跃次数")]
    public int maxJumpCount = 1; //最大跳跃次数


    //维护各个能力脚本
    [HideInInspector]
    public Move MoveScript;
    [HideInInspector]
    public Jump JumpScript;
    [HideInInspector]
    public WallJump WallJumpScript;

    public void Awake()
    {
        //获取刚体组件
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();

        //获取各个能力脚本
        MoveScript = GetComponent<Move>();
        JumpScript = GetComponent<Jump>();
        WallJumpScript = GetComponent<WallJump>();
    }

    public void Update()
    {
        //获取输入（GetAxis有惯性，输入管理器）
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        //float xRaw = Input.GetAxisRaw("Horizontal");
        //float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        if (Move)
        {
            //移动
            MoveScript.Walk(dir);
        }

        if (Jump)
        {
            JumpAction();
        }


    }

    public int jumpCount = 0;
    public void JumpAction()
    {
        //落地或者在墙上就重置跳跃次数
        if((coll.onGround || coll.onLeftWall || coll.onRightWall) && canJump)
        {
            jumpCount = 0;
        }


        //土狼时间（不在地面或者墙面就开始倒计时）
        if (coll.onGround)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
        if (coll.onLeftWall)
        {
            coyoteTimerLeft = coyoteTime;
        }
        else
        {
            coyoteTimerLeft -= Time.deltaTime;
        }
        if (coll.onRightWall)
        {
            coyoteTimerRight = coyoteTime;
        }
        else
        {
            coyoteTimerRight -= Time.deltaTime;
        }

        //跳跃缓冲（按下跳跃键后的短暂时间内就可以跳跃）
        if (Input.GetButtonDown("Jump"))
        {
            bufferTimer = bufferTime;
        }
        else
        {
            bufferTimer -= Time.deltaTime;
        }

        //跳跃
        if (bufferTimer > 0 && canJump)
        {
            if ((coll.onGround || coyoteTimer > 0) || (!coll.onGround && !coll.onLeftWall && !coll.onRightWall && jumpCount < maxJumpCount))
            {
                JumpScript.BasicJump(Vector2.up);

                ++jumpCount;

                coyoteTimer = 0;
                bufferTimer = 0;
            }
            if (!coll.onGround && WallJump)
            {
                //蹬墙跳
                if (coll.onLeftWall || coyoteTimerLeft > 0)
                {
                    WallJumpScript.JumpOnWall(Vector2.left);

                    ++jumpCount;

                    coyoteTimerLeft = 0;
                    bufferTimer = 0;
                }
                else if (coll.onRightWall || coyoteTimerRight > 0)
                {
                    WallJumpScript.JumpOnWall(Vector2.right);

                    ++jumpCount;

                    coyoteTimerRight = 0;
                    bufferTimer = 0;
                }
            }
        }

        //滑落
        if ((coll.onRightWall || coll.onLeftWall) && !coll.onGround)
        {
            wallSlide = true;
            WallJumpScript.SlideOnWall();
        }
        else
        {
            wallSlide = false;
        }
    }
}
