
/*
 Abstractacta para HEREDAR

    recordar que tiene ...
        una funcion que recibe el evento para prender
        una funcion que recibe el evento para apagar
        y otra para Updatear

    NO USAR LOS PUBLICOS como Show y Hide para otra cosa que no sea
    el show y hide propio del interactuable

    El OnUpdate se ejecuta una vez haya sido activado por Show
    y se detiene de la misma manera por Hide
     
*/
using UnityEngine;
public abstract class FeedbackInteractBase : MonoBehaviour
{
    protected bool canupdate = false;
    public void Show() { canupdate = true; OnShow(); }
    public void Hide() { canupdate = false; OnHide(); }
    private void Update() { if (canupdate) OnUpdate(); }
    protected abstract void OnShow();
    protected abstract void OnHide();
    protected abstract void OnUpdate();
}
