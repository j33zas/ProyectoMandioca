using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterBlock : EntityBlock
{
    Action beginparry;
    Action endparry;

    CharacterAnimator anim;
    public bool flagIsStop;
    GameObject feedback;

    Action realblock_on;
    Action realblock_off;

    public CharacterBlock(float timeParry,
                          float blockRange,
                          Action _EndParry,
                          CharacterAnimator _anim,
                          GameObject feedbackBlock
                          /*,Action _realblocOn,
                          Action _realblocOff*/
        ) : base(timeParry, blockRange)
    {


        endparry = _EndParry;
        anim = _anim;
        feedback = feedbackBlock;
    }

    public override void OnBlockDown() { anim.Block(true); }
    public override void OnBlockUp() { anim.Block(false); OnBlockUpSuccessful();  }

    //por animacion
    public override void OnBlockSuccessful()
    {
        Debug.Log("la animacion me dice que puedo bloquear");

        if (!flagIsStop)
        {
            
            feedback.SetActive(true);
            onBlock = true;
        }

        flagIsStop = false;

    }

    //se dispara por input
    public override void OnBlockUpSuccessful()
    {
        // base.OnBlockUpSuccessful();
        // feedback.SetActive(false);
        // Debug.Log("On BLock UP succesful");

        if (onBlock)
        {
            Debug.Log("se dispara cuando suelto la tecla");
            feedback.SetActive(false);
            onBlock = false;
        }
        else
        {
            Debug.Log("se soltó la tecla");
            flagIsStop = true;
        }
    }

    public override void FinishParry()
    {
        base.FinishParry();
        endparry.Invoke();
    }
}
