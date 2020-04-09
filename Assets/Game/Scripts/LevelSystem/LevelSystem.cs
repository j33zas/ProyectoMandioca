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
            Main.instance.gameUiController.UI_SendLevelUpNotification();
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        Main.instance.gameUiController.UI_SendActivePlusNotification(point_to_spend > 0);
        Main.instance.gameUiController.UI_RefreshExpBar(
            currentExpValue, 
            levels[currentIndex].maxt_to_level_up, 
            CURRENT_LEVEL);

    }


    
}
