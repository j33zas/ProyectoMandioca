using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPopUpInWorld_Manager : MonoBehaviour
{

    public static CanvasPopUpInWorld_Manager instance;
    [SerializeField] private List<Icon_DATA> _iconos = new List<Icon_DATA>();
    private static Dictionary<Icon, Icon_DATA> imgRegistry = new Dictionary<Icon, Icon_DATA>();

    [SerializeField]
    private WorldCanvasPopUp popUp_prefab;

    private void Awake()
    {
        instance = this;
        
        for (int i = 0; i < _iconos.Count; i++)
        {
            imgRegistry.Add(_iconos[i].icon, _iconos[i]);
        }
    }

    public WorldCanvasPopUp MakeNewWorldPopUp(Icon icon, Vector3 pos)
    {
        WorldCanvasPopUp newIcon = Instantiate<WorldCanvasPopUp>(popUp_prefab);
        newIcon.SetCanvasPopUp(pos, imgRegistry[icon].image);
        return newIcon;

    }
}

public enum Icon {parry};
