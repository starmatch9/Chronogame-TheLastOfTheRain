using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea : MonoBehaviour
{
    //后续随雨的强度变化
    [Header("海平面上升速度")]
    public float speed = 5f;
    
    bool isRising = false;

    public void StartRising()
    {
        isRising = true;
    }
    public void StopRising()
    {
        isRising = false;
    }

    private void Update()
    {
        if (isRising)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
    }

}
