using UnityEngine;
public class LifeSystem : MonoBehaviour
{
    public CharacterLifeSystem lifesystemExample;
    public FrontendStatBase uilife;

    public int life = 100;

    CustomCamera customcamera;

    private void Start()  
    {
        lifesystemExample = new CharacterLifeSystem();
        lifesystemExample.Config(100, EVENT_OnLoseLife, EVENT_OnGainLife, EVENT_OnDeath, uilife, life);

        customcamera = FindObjectOfType<CustomCamera>();
    }

    void EVENT_OnLoseLife() => Debug.Log("Lose life");
    void EVENT_OnGainLife() => Debug.Log("Gain life");
    void EVENT_OnDeath() => Debug.Log("Death");

    public void Hit(int _val) { 
        lifesystemExample.Hit(_val);
        if (customcamera != null) customcamera.BeginShakeCamera();
    }
    public void Heal(int _val) => lifesystemExample.AddHealth(_val);
    public void Heal_AllHealth() => lifesystemExample.ResetLife();
    public void AddHealth(int _val) => lifesystemExample.IncreaseLife(_val);

}
