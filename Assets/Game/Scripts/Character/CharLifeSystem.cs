using System;
public class CharLifeSystem
{
    LifeSystemBase lifesystem;

    public event Action<int, int> lifechange = delegate { };

    public event Action loselife = delegate { };
    public event Action gainlife = delegate { };
    public event Action death = delegate { };

    public CharLifeSystem(int max, int current)
    {
        lifesystem = new LifeSystemBase();
        lifesystem.Config(max, EVENT_OnLoseLife, EVENT_OnGainLife, EVENT_OnDeath, current);
        lifesystem.AddCallback_LifeChange(OnLifeChange);
    }
    public CharLifeSystem(int max, int current, FrontendStatBase ui)
    {
        lifesystem = new LifeSystemBase();
        lifesystem.Config(max, EVENT_OnLoseLife, EVENT_OnGainLife, EVENT_OnDeath, ui, current);
        lifesystem.AddCallback_LifeChange(OnLifeChange);
    }

    //////////////////////////////////////////////////////////////////////////////////
    /// EVENTS Subscribers
    //////////////////////////////////////////////////////////////////////////////////
    public CharLifeSystem ADD_EVENT_OnChangeValue(Action<int, int> _callback) { lifechange += _callback; return this; }
    public CharLifeSystem REMOVE_EVENT_OnChangeValue(Action<int, int> _callback) { lifechange -= _callback; return this; }
    public CharLifeSystem ADD_EVENT_OnLoseLife(Action _callback) { loselife += _callback; return this; }
    public CharLifeSystem REMOVE_EVENT_OnLoseLife(Action _callback) { loselife -= _callback; return this; }
    public CharLifeSystem ADD_EVENT_OnGainLife(Action _callback) { gainlife += _callback; return this; }
    public CharLifeSystem REMOVE_EVENT_OnGainLife(Action _callback) { gainlife -= _callback; return this; }
    public CharLifeSystem ADD_EVENT_Death(Action _callback) { death += _callback; return this; }
    public CharLifeSystem REMOVE_EVENT_Death(Action _callback) { death -= _callback; return this; }

    //////////////////////////////////////////////////////////////////////////////////
    /// CALLBACKS
    //////////////////////////////////////////////////////////////////////////////////
    void OnLifeChange(int current, int max) => lifechange.Invoke(current, max);
    void EVENT_OnLoseLife() { loselife.Invoke(); }
    void EVENT_OnGainLife() { gainlife.Invoke(); }
    void EVENT_OnDeath() { death.Invoke(); }

    //////////////////////////////////////////////////////////////////////////////////
    /// PUBLIC METHODS
    //////////////////////////////////////////////////////////////////////////////////
    public void Hit(int _val) => lifesystem.Hit(_val);
    public void Heal(int _val) => lifesystem.AddHealth(_val);
    public void Heal_AllHealth() => lifesystem.ResetLife();
    public void AddHealth(int _val) => lifesystem.IncreaseLife(_val);
    public int GetLife() => (int)lifesystem.Life;
    public int GetMax() => lifesystem.GetMax();


}
