using UnityEngine;
public class CharacterAnimator : BaseAnimator
{
    public CharacterAnimator(Animator _anim) : base(_anim) { }
    public void Move(float _speed) => myAnim.SetFloat("Speed", _speed);
    public void Roll() => myAnim.SetTrigger("Roll");
    public void SetVerticalRoll(float x) => myAnim.SetFloat("dirX", x);
    public void SetHorizontalRoll(float y) => myAnim.SetFloat("dirY", y);
    public void Block(bool _block) => myAnim.SetBool("BeginBlock", _block);
    public void BlockSomething() => myAnim.SetTrigger("BlockSomething");
    public void Parry() => myAnim.SetTrigger("Parry");
    public void OnAttackBegin(bool b) => myAnim.SetBool("AttackBegin", b);
    public void NormalAttack() => myAnim.SetTrigger("NormalAttack");
    public void HeavyAttack() => myAnim.SetTrigger("HeavyAttack");
}
