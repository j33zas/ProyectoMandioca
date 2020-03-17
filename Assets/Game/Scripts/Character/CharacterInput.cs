using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInput : MonoBehaviour
{
    //hacer por event system de unity
    //aprobechemos el mismo editor conector
    //por editor le vamos a pasar los inputs al Head

    public static CharacterInput instance;


    [Header("Movement")]
    public UnityEvFloat LeftHorizontal;
    public UnityEvFloat LeftVertical;

    public UnityEvFloat RightHorizontal;
    public UnityEvFloat RightVertical;
                

    private void Awake()
    {
        instance = this;        
    }

    private void Update()
    {
        MoveLeftHorizontal(Input.GetAxis("LeftHorizontal"));

        MoveLeftVertical(Input.GetAxis("LeftVertical"));

        MoveRightHorizontal(Input.GetAxis("RightHorizontal"));

        MoveRightVertical(Input.GetAxis("RightVertical"));
    }    

    void MoveLeftHorizontal(float val)
    {
        LeftHorizontal.Invoke(val);
    }
    void MoveLeftVertical(float val)
    {
        LeftVertical.Invoke(val);
    }
    void MoveRightHorizontal(float val)
    {
        RightHorizontal.Invoke(val);
    }
    void MoveRightVertical(float val)
    {
        RightVertical.Invoke(val);
    }

    [System.Serializable]
    public class UnityEvFloat : UnityEvent<float> { }
}
   

