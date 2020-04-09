using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelSystem : MonoBehaviour
{
    public LevelData[] levels;
    int currentIndex = 0;
    public int CURRENT_LEVEL { get { return currentIndex + 1; } }

    int currentExpValue;

    int point_to_spend;

    public void AddExperiencie(int exp)
    {
        currentExpValue += exp;

        Debug.Log("Experiemncia agregada " + exp + " mi current es: " + currentExpValue);

        if (currentExpValue >= levels[currentIndex].maxt_to_level_up)
        {
            point_to_spend++;
            currentIndex++;
            currentExpValue = 0;
            UI_SendLevelUpNotification();
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        UI_SendActivePlusNotification(point_to_spend > 0);
        UI_RefreshExpBar(
            currentExpValue, 
            levels[currentIndex].maxt_to_level_up, 
            CURRENT_LEVEL);

    }

    /////////////////////////////////////////////////////////////////////////////////////
    /// PARA LAS UIS, si queres sacalo de aca y metelo en un manager mas limpio
    /////////////////////////////////////////////////////////////////////////////////////

    public void UI_SendLevelUpNotification()
    {
        //aca le mando todo el festejo de que subiste de nivel
    }
    public void UI_SendActivePlusNotification(bool val)
    {
        //aca activo o desactivo la lucecita o el algo que indique que puedo elegir una skill
    }
    public void UI_RefreshExpBar(int currentExp, int maxExp, int currentLevel)
    {
        //aca lo mando a una barrita que refresque todo esto
        //y me muestre el nivel y la experienca acumulada
    }
}
