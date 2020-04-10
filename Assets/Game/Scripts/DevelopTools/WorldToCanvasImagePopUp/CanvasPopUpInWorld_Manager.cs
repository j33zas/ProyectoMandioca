using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPopUpInWorld_Manager : MonoBehaviour
{

    public static CanvasPopUpInWorld_Manager instance;//Instancia de manager
    
    [SerializeField] private List<Icon_DATA> _iconos = new List<Icon_DATA>();//lista donde se agregan todas las imagenes y sus IDs ("tupla custom").
                                                                             //Puede ser un scriptable object...
    
    private static Dictionary<Icon, Icon_DATA> imgRegistry = new Dictionary<Icon, Icon_DATA>();//registro de todas las imagenes y sus IDs
    
    [SerializeField] private RectTransform canvasRect;//el canvas donde va a estar ubicado el popUp

    [SerializeField]
    private WorldCanvasPopUp popUp_prefab;//prefab de la imagen

    private void Awake()
    {
        //Singleton
        instance = this;
        
        //Registro las imagenes con su identificador
        for (int i = 0; i < _iconos.Count; i++)
        {
            imgRegistry.Add(_iconos[i].icon, _iconos[i]);
        }
    }
    /// <summary>
    /// Crea y devuelve un PopUp que lo posiciona en la posicion que le mandes con un tipo de icono.
    /// </summary>
    /// <param name="worldObjectPos"></param>
    /// <param name="icon"></param>
    /// <returns></returns>
    public WorldCanvasPopUp MakePopUp(Transform worldObjectPos, Icon icon)
    {
        //Instancia el prefab de la imagen y lo hace hijo del canvas
        WorldCanvasPopUp newPopUp = Instantiate(popUp_prefab, canvasRect.transform);
        //Setea el nuevo objeto. Se le pasa la posicion del objeto del mundo y la imagen dentro del Dic y el canvas donde ubicarla.
        newPopUp.SetCanvasPopUp(worldObjectPos, imgRegistry[icon].image, canvasRect);
        return newPopUp;
    }
    
    public WorldCanvasPopUp MakePopUpAnimated(Transform worldObjectPos, WorldCanvasPopUp icon)
    {
        //Instancia el prefab de la imagen y lo hace hijo del canvas
        WorldCanvasPopUp newPopUp = Instantiate(icon, canvasRect.transform);
        //Setea el nuevo objeto. Se le pasa la posicion del objeto del mundo y la imagen dentro del Dic y el canvas donde ubicarla.
        newPopUp.SetCanvasPopUp(worldObjectPos, canvasRect);
        return newPopUp;
    }
    
}
//IDs de las diferentes imagens de popUp
public enum Icon {parry};
