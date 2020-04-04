using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetrifyTEST : MonoBehaviour
{
    public SkillPetrify skillPetrify;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            skillPetrify.BeginSkill();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            skillPetrify.EndSkill();
        }
    }
}
