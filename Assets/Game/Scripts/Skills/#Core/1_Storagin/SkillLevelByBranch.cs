using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillLevelByBranch", menuName = "Skills/LevelByBranch", order = 2)]
public class SkillLevelByBranch : ScriptableObject
{
    public SkillType skilltype;
    public SkillInfo[] Selection;
    public SkillInfo[] aleatorios;
}