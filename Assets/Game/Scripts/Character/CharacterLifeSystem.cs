using UnityEngine;
using System;
using Event = Life.EV_LIFE;

public class CharacterLifeSystem : MonoBehaviour
{
    Life life;
    public FrontendLifeBase uilife;

    public void Config(int life_count_Max, Action OnLoseLife, Action OnGainLife, Action OnDeath, int initial_life = -1)
    {
        if (uilife == null) return;
        life = new Life(life_count_Max, uilife, initial_life);
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
                return life.Health;
            }
            else
            {
                return 0;
            }
        }
    }

    public void SetCurrentLifeNoMax(int currentlife) => life.Health = currentlife;
    public int GetMax() => life.MaxHealth;
    public void Hit() => life.Health--;
    public void Hit(int val) => life.Health -= val;
    public void AddHealth() => life.Health++;
    public void AddHealth(int val) => life.Health += val;
    public bool CanHealth() => life.Health < life.MaxHealth;
    public void IncreaseLife() => life.IncreaseLife(1);
    public void IncreaseLife(int val) => life.IncreaseLife(val);
    public void SetLife(int newMax) => life.SetLife(newMax);
    public void ResetLife() => life.ResetLife();
    public override string ToString() => life.Health.ToString();
}
