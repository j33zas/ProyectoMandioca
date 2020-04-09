using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlUpSkillSelection_UI : MonoBehaviour
{
    [SerializeField] private Text skillDescription;
    List<SkillInfo> _skills = new List<SkillInfo>();
    [SerializeField] private RectTransform container;

    [SerializeField] private Skill_Btt_template _skillBttTemplate_pf;
    [SerializeField] private Button finishSelection_btt;

    private SkillInfo currentSkillSelected;

    private Action<SkillInfo> OnFinishSelection; 

    public void Configure(List<SkillInfo> skills, Action<SkillInfo> callback)
    {
        OnFinishSelection = callback;
        finishSelection_btt.onClick.AddListener(FinishSelection);
        Populate(skills);
        
    }

    void FinishSelection()
    {
        OnFinishSelection.Invoke(currentSkillSelected);
        StartCoroutine(SelfDestroy());
    }
    
    private void Populate(List<SkillInfo> skills)
    {
        bool isFirst = false;
        foreach (SkillInfo sk in skills)
        {
            var newButton = Instantiate(_skillBttTemplate_pf, container);
            newButton.SetData(sk, SelectSkill);

            if (!isFirst)
            {
                isFirst = true;
                newButton.GetComponent<Button>().Select(); ;
                currentSkillSelected = sk;
            }
        }
    }

    private void Update()
    {
        if (currentSkillSelected == null)
            finishSelection_btt.interactable = false;
        else
            finishSelection_btt.interactable = true;
        
        if (currentSkillSelected == null)
            skillDescription.text = "Elija un skill para ver el detalle";
        else
        {
            skillDescription.text = currentSkillSelected.description_technical;
        }
    }

    private void SelectSkill(SkillInfo selectedSkill)
    {
        currentSkillSelected = selectedSkill;
    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
