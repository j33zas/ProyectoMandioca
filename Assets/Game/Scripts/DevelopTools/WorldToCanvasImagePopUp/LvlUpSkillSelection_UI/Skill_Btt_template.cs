using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Skill_Btt_template : MonoBehaviour
{
    private SkillInfo allocatedSkill;

    private Action<SkillInfo> OnSelected;

    public void SetData(SkillInfo skill, Action<SkillInfo> OnSelected)
    {
        allocatedSkill = skill;
        this.OnSelected = OnSelected;

        GetComponent<Button>().image.sprite = allocatedSkill.img_actived;
        
        GetComponent<Button>().onClick.AddListener(SelectSkill);
    }

    private void SelectSkill()
    {
        OnSelected(allocatedSkill);
    }

    public void OnSelect(BaseEventData eventData)
    {
        
    }
}
