using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInput : MonoBehaviour
{
    public enum InputType { Joystick, Mouse, Other }
    public InputType input_type;

    [Header("Movement")]
    public UnityEvFloat LeftHorizontal;
    public UnityEvFloat LeftVertical;
    public UnityEvFloat RightHorizontal;
    public UnityEvFloat RightVertical;
    public UnityEvent Dash;

    private void Update()
    {
        LeftHorizontal.Invoke(Input.GetAxis("Horizontal"));
        LeftVertical.Invoke(Input.GetAxis("Vertical"));
        if (input_type == InputType.Joystick) JoystickInputs();
        else if(input_type == InputType.Mouse) MouseInputs();
        if (Input.GetButtonDown("Dash")) Dash.Invoke();
    }

    public void MouseInputs()
    {
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 mousePos = (mouseOnScreen - positionOnScreen).normalized;
        RightHorizontal.Invoke(mousePos.x);
        RightVertical.Invoke(mousePos.y);
    }

    public void JoystickInputs()
    {
        RightHorizontal.Invoke(Input.GetAxis("RightHorizontal"));
        RightVertical.Invoke(Input.GetAxis("RightVertical"));
    }

    [System.Serializable]
    public class UnityEvFloat : UnityEvent<float> { }
}
   

