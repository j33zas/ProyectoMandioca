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
        _image = GetComponent<Image>();
        _pos = GetComponent<RectTransform>();
    }
    
    public void SetCanvasPopUp(Vector2 posInCanvas, Sprite img)
    {
        _image.sprite = img;
        _pos.anchoredPosition = posInCanvas;
    }
    
}
