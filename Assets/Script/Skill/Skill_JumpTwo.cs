using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_JumpTwo : Skill
{
    public override void UseSkill()
    {
        skillManager.SkillTime(1);
    }
}
