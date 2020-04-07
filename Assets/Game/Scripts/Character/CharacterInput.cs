﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterInput : MonoBehaviour
{
    public enum InputType { Joystick, Mouse, Other }
    public InputType input_type;

    JoystickBasicInput joystickhelper;
    
    [Header("Movement")]
    public UnityEvFloat LeftHorizontal;
    public UnityEvFloat LeftVertical;
    public UnityEvFloat RightHorizontal;
    public UnityEvFloat RightVertical;
    public UnityEvFloat ChangeWeapon;
    public UnityEvent Dash;

    [Header("Defense")]
    public UnityEvent OnBlock;
    public UnityEvent UpBlock;
    public UnityEvent Parry;

    [Header("Attack")]
    public UnityEvent OnAttack;
    public UnityEvent OnAttackEnd;

    [Header("Interact")]
    public UnityEvent OnInteract;
    public UnityEvent Back;

    [Header("Test pasives")]
    public UnityEvent OnDpad_Up;
    public UnityEvent OnDpad_Down;
    public UnityEvent OnDpad_Left;
    public UnityEvent OnDpad_Right;

    private void Awake() => ConfigureJoystickHelper();
    private void Update()
    {
        LeftHorizontal.Invoke(Input.GetAxis("Horizontal"));
        LeftVertical.Invoke(Input.GetAxis("Vertical"));
        if (input_type == InputType.Joystick) JoystickInputs();
        else if (input_type == InputType.Mouse) MouseInputs();
        if (Input.GetButtonDown("Dash")) Dash.Invoke();

        if (Input.GetButtonDown("Block")) OnBlock.Invoke();
        if (Input.GetButtonUp("Block")) UpBlock.Invoke();
        if (Input.GetButtonDown("Parry")) Parry.Invoke();
        
        if (Input.GetButtonDown("Attack")) OnAttack.Invoke();
        if (Input.GetButtonUp("Attack")) OnAttackEnd.Invoke();

        if (Input.GetButtonDown("Interact")) OnInteract.Invoke();
        ChangeWeapon.Invoke(Input.GetAxis("XBOX360_DPadHorizontal"));

        if (Input.GetButtonDown("Back")) Back.Invoke();

        RefreshHelper();

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

    public void ChangeRotationInput()
    {
        if (input_type == InputType.Mouse) input_type = InputType.Joystick;
        else if (input_type == InputType.Joystick) input_type = InputType.Mouse;
    }

    #region JoystickHelper
    void ConfigureJoystickHelper()
    {
        joystickhelper = new JoystickBasicInput();
        joystickhelper
            .SUBSCRIBE_DPAD_UP(EV_DPAD_UP)
            .SUBSCRIBE_DPAD_DOWN(EV_DPAD_DOWN)
            .SUBSCRIBE_DPAD_RIGHT(EV_DPAD_RIGHT)
            .SUBSCRIBE_DPAD_LEFT(EV_DPAD_LEFT);
    }
    void RefreshHelper() => joystickhelper.Refresh();
    void EV_DPAD_UP() => OnDpad_Up.Invoke();
    void EV_DPAD_DOWN() => OnDpad_Down.Invoke();
    void EV_DPAD_LEFT() => OnDpad_Left.Invoke();
    void EV_DPAD_RIGHT() => OnDpad_Right.Invoke();
    #endregion


    [System.Serializable]
    public class UnityEvFloat : UnityEvent<float> { }
}


