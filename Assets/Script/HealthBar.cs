using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("血条组件")]
    public Image fillImage;

    public void SetHealth(float healthPercentage)
    {
        //此方法将值控制在0到1之间，杜绝负值和超过1的情况
        healthPercentage = Mathf.Clamp01(healthPercentage);
        //设置填充图像的填充量
        fillImage.fillAmount = healthPercentage;
    }

}
