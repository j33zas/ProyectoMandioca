using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBlock
{
    public bool onBlock;
    public bool onParry;
    protected float timeToParry;
    protected float timer;
    protected bool canupdate;

    //Mucho muy importante que sea de -1 a 1
    float blockAngle;

    public EntityBlock(float timeParry, float blockRange)
    {
        timeToParry = timeParry;
        blockAngle = blockRange;
    }

    public virtual void OnBlockDown() { }
    public virtual void OnBlockUp() { }
    public virtual void OnBlockSuccessful() => onBlock = true;
    public virtual void OnBlockUpSuccessful() => onBlock = false;

    public virtual bool IsParry(Vector3 parryDir, Vector3 attackDir)
    {
        if (onParry)
        {
            float blockRange = Vector3.Dot(parryDir.normalized, attackDir.normalized);

            if (blockRange <= blockAngle)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public virtual bool IsBlock(Vector3 blockDir, Vector3 attackDir)
    {
        if (onBlock)
        {
            float blockRange = Vector3.Dot(blockDir.normalized, attackDir.normalized);

            if (blockRange <= blockAngle)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public virtual void Parry()
    {
        if (!canupdate)
        {
            onParry = true;
            canupdate = true;
        }
    }

    public void OnUpdate()
    {
        if (canupdate)
        {
            timer += Time.deltaTime;
            if (timer >= timeToParry)
            {
                FinishParry();
                onParry = false;
                canupdate = false;
                timer = 0;
            }
        }
    }

    public virtual void FinishParry() { }
}
