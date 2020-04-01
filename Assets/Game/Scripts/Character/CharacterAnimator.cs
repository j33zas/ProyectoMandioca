using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : BaseAnimator
{
    public CharacterAnimator(Animator _anim) : base(_anim) { }
    public void Move(float _speed) => myAnim.SetFloat("Speed", _speed);
    public void Roll() => myAnim.SetTrigger("Roll");
    public void Block(bool _block) => myAnim.SetBool("BeginBlock", _block);
    public void BlockSomething() => myAnim.SetTrigger("BlockSomething");
    public void Parry() => myAnim.SetTrigger("Parry");
    public void OnAttackBegin() => myAnim.SetTrigger("AttackBegin");
    public void NormalAttack() => myAnim.SetTrigger("NormalAttack");
    public void HeavyAttack() => myAnim.SetTrigger("HeavyAttack");
}
