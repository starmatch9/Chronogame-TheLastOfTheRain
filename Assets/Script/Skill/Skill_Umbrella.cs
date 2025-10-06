using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Umbrella : Skill
{
    public override void UseSkill()
    {
        skillManager.SkillTime(0);
    }
}
