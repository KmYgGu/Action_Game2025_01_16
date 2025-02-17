using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActiveSkill
{
    void Active();
    string SkillName();

    int ManaCost { get; }
    int CoolDown { get; }
}
    
