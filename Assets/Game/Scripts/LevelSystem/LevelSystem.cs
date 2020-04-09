using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public LevelData[] levels;
    int currentLevel = 1;

    int currentExpValue;

    int point_to_spend;

    public void AddExperiencie(int exp)
    {
        currentExpValue += exp;
        //frontend.refresh(levels[currentlevel].max, currentExpValue)

        if (currentExpValue >= levels[currentLevel].maxt_to_level_up)
        {
            point_to_spend++;
            currentLevel++;
            currentExpValue = 0;
            UI_SendLevelUpNotification();
        }

        RefreshUI();
    }


    public void RefreshUI()
    {
        UI_SendActivePlusNotification(point_to_spend > 0);
        UI_RefreshExpBar(currentExpValue, levels[currentLevel].maxt_to_level_up);

    }
    public void UI_SendLevelUpNotification()
    {
        //aca le mando todo el festejo de que subiste de nivel
    }
    public void UI_SendActivePlusNotification(bool val)
    {
        //aca activo o desactivo la lucecita o el algo que indique que puedo elegir una skill
    }
    public void UI_RefreshExpBar(int currentExp, int maxExp)
    {

    }
}
