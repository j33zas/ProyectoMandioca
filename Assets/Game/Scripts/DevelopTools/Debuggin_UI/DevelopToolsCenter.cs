using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools.UI;
using Tools;

public class DevelopToolsCenter : MonoBehaviour
{
    bool open = false;
    public UI_GraphicContainer ui_gc;

    int numexample;
    int max = 5;

    public int Numexample { 
        get => numexample;
        set => numexample = value < 0 ? 0 : (value > max ? max: value);
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

    private void Start()
    {
        Configurations();
    }

    void Configurations()
    {
        Debug_UI_Tools.instance.CreateSlider("Example", 2, 0, 10, Example);
        Debug_UI_Tools.instance.CreateToogle("example 2", true, Example);
    }

    string Example(float f)
    {
        return "s=" + f;
    }

    string Example(bool f)
    {
        return "b=" + f;
    }
}
