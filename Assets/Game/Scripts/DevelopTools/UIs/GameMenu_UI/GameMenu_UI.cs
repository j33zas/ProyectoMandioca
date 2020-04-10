using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu_UI : MonoBehaviour
{
    List<SkillInfo> _skillInfos = new List<SkillInfo>();
    private SkillManager_Pasivas _skillManagerPasivas;

    [SerializeField] private RectTransform passiveSkills_container;
    [SerializeField] private RectTransform passiveSkillsSelection_container;
    [SerializeField] private PassiveSkill_template ps_template_pf;
    [SerializeField] private LvlUpSkillSelection_UI psSelection_template_pf;

    [SerializeField] private Text skillDescription_txt;
    
    private void Start()
    {
        _skillManagerPasivas = FindObjectOfType<SkillManager_Pasivas>();

        if (_skillManagerPasivas != null)
        {
            _skillInfos = _skillManagerPasivas.GetCurrentPassiveSkills();
            PopulatePassiveSkills();
        }
        
        SetSkillSelection();
    }

    void RegistryButtons()
    {
        //Registro las acciones de los botones del menu
    }
    
    void PopulatePassiveSkills()
    {
        bool first = false;
        //Pongo todos los skills que tengo pasivos
        foreach (SkillInfo sI in _skillInfos)
        {
            PassiveSkill_template newSkill = Instantiate(ps_template_pf, passiveSkills_container);
            newSkill.Configure(sI, UpdateSkillDescription);
            
            if (!first)
            {
                first = true;
                newSkill.Select();
            }
        }
    }

    void SetSkillSelection()
    {
        Debug.Log("entro a ver si hay skill");
        //Aca veo si tengo que elegir skill o no
        if (_skillManagerPasivas.I_Have_An_Active_Request())
        {
            var newSelection = Instantiate(psSelection_template_pf, passiveSkillsSelection_container);
            newSelection.Configure(_skillManagerPasivas.GetSkillRequest() ,_skillManagerPasivas.ReturnSkill);
        }
    }

    void UpdateSkillDescription(SkillInfo skill)
    {
        //Dependiendo el currentSelected, tengo que escribir el texto de su descripcion
        skillDescription_txt.text = skill.description_technical;

    }

    void Refresh()
    {
        //Hago todo denuevo por si hay algun cambio
    }
    
}
