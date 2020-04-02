using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPopUp : MonoBehaviour
{
    public Transform playerPos;

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
            
            testPopUp = CanvasPopUpInWorld_Manager.instance.MakePopUp( playerPos.transform.position + Vector3.up * 3, Icon.parry);
        }
    }
}
