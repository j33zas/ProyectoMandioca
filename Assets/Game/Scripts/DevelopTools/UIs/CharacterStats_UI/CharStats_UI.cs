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
    }
    
    public void UpdateXP_UI(int current, int maxXP, int currentLvl)
    {
        currentXp_Bar.fillAmount = current / maxXP;

        currentLvl_txt.text = "Lvl " + currentLvl;
    }
}
