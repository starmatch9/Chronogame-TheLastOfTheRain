using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//卷轴滚动脚本（模仿Cinemation）

//   * LateUpdate *
//LateUpdate() 在所有 Update() 执行后调用,适用于依赖于其他对象更新的脚本
//

public class FollowTarget : MonoBehaviour
{
    [Header("目标游戏对象")]
    public Transform target;

    [Header("相关参数")]
    public float followSpeed = 5f;
    public float deadZoneTop = 2f; //死区顶部
    public float deadZoneBottom = 4f; //死区底部

    void LateUpdate()
    {
        if (target == null){
            return;
        }

        //维护相机的当前位置
        Vector3 currentPos = transform.position;

        //维护相机的目标位置（只用维护y轴）
        Vector3 targetPos = currentPos;

        //计算角色与相机的垂直距离
        float verticalDiff = target.position.y - currentPos.y;

        //超出死区范围，移动相机
        if (Mathf.Abs(verticalDiff) > deadZoneTop && verticalDiff > 0)
        {
            targetPos.y = target.position.y - deadZoneTop;
        }
        else if (Mathf.Abs(verticalDiff) > deadZoneBottom && verticalDiff < 0)
        {
            targetPos.y = target.position.y + deadZoneBottom;  //理解：死区坐标加上角色坐标够到摄像机坐标
        }

        transform.position = Vector3.Lerp(currentPos, targetPos, followSpeed * Time.deltaTime);
    }
}
