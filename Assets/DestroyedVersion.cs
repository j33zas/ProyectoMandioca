using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedVersion : MonoBehaviour
{
    public Renderer[] render;

    float timer;

    bool animate;
    Color mycolor;

    public void BeginDestroy()
    {
        render = GetComponentsInChildren<Renderer>();
        mycolor = render[0].material.GetColor("_Color");
        animate = true;
        timer = 1;
    }

    private void Update()
    {
        if (animate)
        {
            if (timer > 0)
            {
                timer = timer - 0.3f * Time.deltaTime;
                foreach (var r in render) r.material.SetColor("_Color", new Color(mycolor.r, mycolor.g, mycolor.b, timer));
            }
            else
            {
                animate = false;
                timer = 1;
                Destroy(this.gameObject);
            }
        }
    }
}
