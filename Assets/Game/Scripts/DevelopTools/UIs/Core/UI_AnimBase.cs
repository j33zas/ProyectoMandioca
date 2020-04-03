using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class UI_AnimBase : MonoBehaviour
{
    Action EndOpenAnimation;
    Action EndCloseAnimation;

    [Range(1,20)]
    public float speed = 9;

    float timer = 0;
    const float time_to_go = 1;
    bool anim;
    bool go;

    public void AddEvents(Action EV_End_OpenAnimation, Action EV_End_CloseAnimation)
    {
        EndOpenAnimation = EV_End_OpenAnimation;
        EndCloseAnimation = EV_End_CloseAnimation;
    }

    public void Open()
    {
        anim = true;
        go = true;
    }
    public void Close()
    {
        anim = true;
        go = false;
    }

    public abstract void OnGo(float time_value);
    public abstract void OnBack(float time_value);

    private void Update()
    {
        if (anim)
        {
            if (timer < time_to_go)
            {
                timer = timer + speed * Time.deltaTime;

                if (go)
                {
                    OnGo(timer);
                }
                else
                {
                    OnBack(timer);
                }
            }
            else
            {
                if (go) {  EndOpenAnimation(); }
                else {  EndCloseAnimation(); }
                timer = 0;
                anim = false;
            }
        }
    }
}
