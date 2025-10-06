using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbFollow : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(GlobalData.player.transform.position.x, GlobalData.player.transform.position.y, 0f);
    }
}
