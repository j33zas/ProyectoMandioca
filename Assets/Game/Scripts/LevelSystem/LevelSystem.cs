using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LevelSystem : MonoBehaviour
{
    public LevelData[] levels;
    int currentIndex = 0;
    public int CURRENT_LEVEL { get { return currentIndex + 1; } }
    int currentExpValue;
    //int point_to_spend;

    Func<bool> I_have_an_active_request;
    public void Initialize()
    {
        I_have_an_active_request = Main.instance.skillmanager_pasivas.I_Have_An_Active_Request;
    }

    public void AddExperiencie(int exp)
    {
        currentExpValue += exp;

        if (currentExpValue >= levels[currentIndex].maxt_to_level_up)
        {
            //point_to_spend++;
            currentExpValue = 0;
            if (levels[currentIndex].can_get_skill_point)
            {
                Main.instance.skillmanager_pasivas.CreateRequest_NewSkill();
            }
            Main.instance.gameUiController.UI_SendLevelUpNotification();
            currentIndex++;
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        Main.instance.gameUiController.UI_SendActivePlusNotification(I_have_an_active_request());
        Main.instance.gameUiController.UI_RefreshExpBar(
            currentExpValue, 
            levels[currentIndex].maxt_to_level_up, 
            CURRENT_LEVEL);

    }
}
