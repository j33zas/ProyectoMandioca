using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class Skill_Btt_template : MonoBehaviour, ISelectHandler
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
        Debug.Log("caca");
    }
}
