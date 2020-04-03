using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPopUp : MonoBehaviour
{
    public Transform objInWorld;

    private WorldCanvasPopUp testPopUp;
    
    
    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (testPopUp != null)
            {
                Destroy(testPopUp.gameObject);
                return;
            }
            
            testPopUp = CanvasPopUpInWorld_Manager.instance.MakePopUp( objInWorld, Icon.parry);
        }
    }
}
