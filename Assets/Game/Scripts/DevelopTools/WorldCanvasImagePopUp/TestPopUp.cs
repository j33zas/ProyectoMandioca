using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPopUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CanvasPopUpInWorld_Manager.instance.MakeNewWorldPopUp(Icon.parry, new Vector3(2, 2, 2));
        }
    }
}
