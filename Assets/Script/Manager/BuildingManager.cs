using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [Header("鼠标跟随对象")]
    public MouseFollow mouseFollow;

    [Header("建筑物父物体")]
    public Transform buildingParent;

    [Header("可用建筑物")]
    public List<GameObject> buildings = new List<GameObject>();

    private void Awake()
    {
        GlobalData.buildings = buildings;

        GlobalData.mouseFollow = mouseFollow;

        GlobalData.buildingParent = buildingParent;
    }
}
