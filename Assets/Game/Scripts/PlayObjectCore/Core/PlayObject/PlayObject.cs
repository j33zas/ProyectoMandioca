using UnityEngine;

public abstract class PlayObject : MonoBehaviour
{

    private bool canupdate;
    public void On() { canupdate = true; OnTurnOn(); }
    public void Off() { canupdate = false; OnTurnOff(); }

    public void Pause() { OnPause(); }
    public void Resume() { OnResume(); }

    protected abstract void OnTurnOn();
    protected abstract void OnTurnOff();
    protected abstract void OnUpdate();
    protected abstract void OnFixedUpdate();
    protected abstract void OnPause();
    protected abstract void OnResume();

    private void Update() { if (canupdate) OnUpdate(); }
    private void FixedUpdate() { if (canupdate) OnFixedUpdate(); }

}
