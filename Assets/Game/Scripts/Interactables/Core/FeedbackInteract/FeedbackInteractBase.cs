using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FeedbackInteractBase : MonoBehaviour
{
    bool canupdate = false;

    public void Show()
    {
        canupdate = true;
        OnShow();
    }
    public void Hide()
    {
        canupdate = false;
        OnHide();
    }

    private void Update()
    {
        if (canupdate)
        {
            OnUpdate();
        }
    }


    protected abstract void OnShow();
    protected abstract void OnHide();
    protected abstract void OnUpdate();
}
