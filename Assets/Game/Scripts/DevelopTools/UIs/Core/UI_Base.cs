using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UI_GoBack))]
public abstract class UI_Base : MonoBehaviour
{
    public int idfinder;

    public bool isActive;
    public GameObject firstToOpenMenu;

    public void SetSpeed(float speed) { anim.speed = speed; }

    UI_GoBack anim;
    [SerializeField] protected GameObject parent;
    void Awake()
    {
        anim = GetComponent<UI_GoBack>();
        if (anim == null) throw new System.Exception("No contiene un UI_GoBack");
        else anim.AddEvents(OnEndOpenAnimation, EndCloseAnimation);
        OnAwake();
    }
    void Start()
    {
        //if (!anim.test_stay_in_my_place) parent.SetActive(false);
        OnStart();
    }
    void EndCloseAnimation()
    {
        //parent.SetActive(false);
        OnEndCloseAnimation();
    }

    void Update() { OnUpdate(); }
    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnEndOpenAnimation();
    protected abstract void OnEndCloseAnimation();
    protected abstract void OnUpdate();
    public abstract void Refresh();
    
    public void OnForSettings() { parent.SetActive(true); }
    public void OFFForSettings() { parent.SetActive(false); }
    public void PreOpen() { parent.SetActive(true); }

    public void ConfigurateFirst(GameObject go)
    {
        firstToOpenMenu = go;
    }

    public virtual void Open()
    {
        anim.Open();
        parent.SetActive(true);
        Refresh();
        isActive = true;
        Main.instance.GetMyEventSystem().Set_First(firstToOpenMenu.gameObject);
    }
    public void Close()
    {
        anim.Close();
        /*isActive = false; */
        Main.instance.GetMyEventSystem().DeselectGameObject();
    }
}
