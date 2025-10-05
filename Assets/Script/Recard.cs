using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recard : MonoBehaviour
{
    //记录当前人物的最高高度
    public int highestY = 0;

    private void Update()
    {
        if( (int)transform.position.y + 5 > highestY)
        {
            highestY = (int)transform.position.y + 5;
            GlobalData.recard_UI.UpdateData();
        }
    }

}
