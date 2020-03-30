using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : BaseAnimator
{
    public CharacterAnimator(Animator _anim) : base(_anim)
    {

    }

    public void Move(float _speed)
    {
        myAnim.SetFloat("Speed", _speed);
    }

    public void Roll()
    {
        myAnim.SetTrigger("Roll");
    }
}
