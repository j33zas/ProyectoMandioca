using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GoBack : UI_AnimBase
{
    public bool test_stay_in_my_place;

    Vector3 currentpos;
    Vector3 hidepos;

    public enum AppearSide { Up, Down, Left, Right }
    public AppearSide side;

    private void Start()
    {
        currentpos = transform.localPosition;
        switch (side) {
            case AppearSide.Up: hidepos = new Vector3(transform.localPosition.x, transform.localPosition.y + 1000, transform.localPosition.z); break;
            case AppearSide.Down: hidepos = new Vector3(transform.localPosition.x, transform.localPosition.y - 1000, transform.localPosition.z); break;
            case AppearSide.Left: hidepos = new Vector3(transform.localPosition.x - 1000, transform.localPosition.y, transform.localPosition.z); break;
            case AppearSide.Right: hidepos = new Vector3(transform.localPosition.x + 1000, transform.localPosition.y, transform.localPosition.z); break;
        }
        if(!test_stay_in_my_place) transform.localPosition = hidepos;

    }

    public override void OnGo(float time_value) { transform.localPosition = Vector3.Lerp(hidepos, currentpos, time_value); }
    public override void OnBack(float time_value) { transform.localPosition = Vector3.Lerp(currentpos, hidepos, time_value); }
}
