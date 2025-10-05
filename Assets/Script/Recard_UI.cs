using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recard_UI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI highestYText;

    public void Awake()
    {
        GlobalData.recard_UI = this;
    }

    public void UpdateData()
    {
        highestYText.text = "当前抵达的最大高度： " + GlobalData.recard.highestY.ToString() + "米";
    }
}
