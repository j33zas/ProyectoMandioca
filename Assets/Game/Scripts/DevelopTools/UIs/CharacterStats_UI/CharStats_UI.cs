using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class CharStats_UI : MonoBehaviour
{
    private float currentLife;
    private float maxHP;

    private CharacterHead _hero;
    private CharacterLifeSystem heroLifeSystem;

    [SerializeField] private Image currentLife_Bar;
    [SerializeField] private Image currentXp_Bar;
    [SerializeField] private Text currentPath_txt;//tengo que ver de donde agarro esto
    [SerializeField] private Text currentLvl_txt;
    [SerializeField] private Text hp_txt;
    [SerializeField] private Text xp_txt;
    [SerializeField] private GameObject lvlUpSign; 
    [SerializeField] private GameObject skills_container;

    [SerializeField] private GameObject skillImage_template_pf;


    private void Start()
    {
        _hero = Main.instance.GetChar();

        heroLifeSystem = _hero.GetCharacterLifeSystem();
        
        
        maxHP = heroLifeSystem.GetMax();
    }

    private void Update()
    {
        UpdateLife_UI(heroLifeSystem.Life);
    }

    public void UpdateLife_UI(float newValue)
    {
        currentLife_Bar.fillAmount = newValue / maxHP;
        hp_txt.text = newValue + " / " + maxHP;
    }
    
    public void UpdateXP_UI(int current, int maxXP, int currentLvl)
    {

        float cur = current;
        float max = maxXP;
        
        currentXp_Bar.fillAmount = cur / max;

        xp_txt.text = current + " / " + maxXP;
        
        currentLvl_txt.text = "Lvl " + currentLvl;
    }

    public void ToggleLvlUpSign()
    {
        if (lvlUpSign.activeInHierarchy)
            return;
        
        lvlUpSign.SetActive(!lvlUpSign.activeInHierarchy);
    }

    public void UpdatePasiveSkills(List<SkillInfo> skillsNuevas)
    {
        foreach (SkillInfo si in skillsNuevas)
        {
            GameObject newSkill = Instantiate(skillImage_template_pf, skills_container.transform);
            newSkill.GetComponent<Image>().sprite = si.img_actived;
        }
    }
}
