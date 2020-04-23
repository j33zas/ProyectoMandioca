using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools.UI;
using Tools;
using System.Linq;

public class DevelopToolsCenter : MonoBehaviour
{
    public static DevelopToolsCenter instance; private void Awake() => instance = this;

    bool open = false;
    public UI_GraphicContainer ui_gc;

    int numexample;
    int max = 5;

    public int Numexample { 
        get => numexample;
        set => numexample = value < 0 ? 0 : (value > max ? max: value);
    }
    
    private void Start()
    {
        ToogleDebug(false);
        Refresh();
        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Dummy Enemy State Machine Debug", false, ToogleDebug);
    }
    public void UIBUTTON_WrenchDebug()
    {
        open = !open;
        Debug_UI_Tools.instance.Toggle(open);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Numexample++;
            Refresh();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Numexample--;
            Refresh();
        }
    }

    void Refresh()
    {
        ui_gc.OnValueChange(Numexample, max, true);
    }

    

    void Configurations()
    {
        
    }
    bool enemydebug;
    public bool EnemyDebuggingIsActive() { return enemydebug; }
    string ToogleDebug(bool active) { enemydebug = active; FindObjectsOfType<TrueDummyEnemy>().ToList().ForEach(x => x.ToogleDebug(active)); return active ? "debug activado" : "debug desactivado"; }

}
