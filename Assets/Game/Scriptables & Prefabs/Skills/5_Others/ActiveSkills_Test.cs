using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkills_Test : MonoBehaviour
{
    [SerializeField] private SkillActivas testSkill;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            testSkill.BeginSkill();
            testSkill.Execute();
        }
    }
}
