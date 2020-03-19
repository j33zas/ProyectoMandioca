using UnityEngine;
using System;

public class CharacterLifeSystem : MonoBehaviour
{
    LifeBase life;
    public FrontendStatBase uilife;

    public void Config(int life_count_Max, Action OnLoseLife, Action OnGainLife, Action OnDeath, int initial_life = -1)
    {
        if (uilife == null) return;
        life = new LifeBase(life_count_Max, uilife, initial_life);
        life.AddEventListener_Death(OnDeath);
        life.AddEventListener_GainLife(OnGainLife);
        life.AddEventListener_LoseLife(OnLoseLife);
    }

    public float Life
    {
        get
        {
            if (life != null)
            {
                return life.Val;
            }
            else
            {
                return 0;
            }
        }
    }

    public void SetCurrentLifeNoMax(int currentlife) => life.Val = currentlife;
    public int GetMax() => life.MaxVal;
    public void Hit() => life.Val--;
    public void Hit(int val) => life.Val -= val;
    public void AddHealth() => life.Val++;
    public void AddHealth(int val) => life.Val += val;
    public bool CanHealth() => life.Val < life.MaxVal;
    public void IncreaseLife() => life.IncreaseValue(1);
    public void IncreaseLife(int val) => life.IncreaseValue(val);
    public void SetLife(int newMax) => life.SetValue(newMax);
    public void ResetLife() => life.ResetValueToMax();
    public override string ToString() => life.Val.ToString();
}
