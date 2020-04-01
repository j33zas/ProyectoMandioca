using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldCanvasPopUp : MonoBehaviour
{
    private Image _image;
    private RectTransform _pos;

    private void Awake()
    {
        _pos = GetComponent<RectTransform>();
    }
    
    public void SetCanvasPopUp(Vector3 pos, Image img)
    {
        _image = img;
        _pos.transform.position = pos;
    }
    
}
