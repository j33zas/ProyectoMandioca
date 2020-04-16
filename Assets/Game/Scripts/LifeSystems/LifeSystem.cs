using UnityEngine;
using Tools.EventClasses;
public class LifeSystem : MonoBehaviour
{
    public CharacterLifeSystem lifesystemExample;
    public FrontendStatBase uilife;

    public int life = 100;

    CustomCamera customcamera;

    public EventTwoInt OnChangeValue;

    private void Awake()  
    {
        lifesystemExample = new CharacterLifeSystem();
        lifesystemExample.Config(100, EVENT_OnLoseLife, EVENT_OnGainLife, EVENT_OnDeath, uilife, life);

        customcamera = FindObjectOfType<CustomCamera>();

        lifesystemExample.AddCallback_LifeChange(OnLifeChange);
    }

    void OnLifeChange(int current, int max)
    {
        OnChangeValue.Invoke(current, max);
    }

    void EVENT_OnLoseLife() { }
    void EVENT_OnGainLife() { }
    void EVENT_OnDeath() { }

    public void Hit(int _val) { 
        lifesystemExample.Hit(_val);
        if (customcamera != null) customcamera.BeginShakeCamera();
    }
    public void Heal(int _val) => lifesystemExample.AddHealth(_val);
    public void Heal_AllHealth() => lifesystemExample.ResetLife();
    public void AddHealth(int _val) => lifesystemExample.IncreaseLife(_val);

}
