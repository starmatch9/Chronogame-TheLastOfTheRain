using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public SkillManager skillManager = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            UseSkill();
            //销毁技能物体
            Destroy(gameObject);
        }
    }

    public virtual void UseSkill()
    {

    }
}
