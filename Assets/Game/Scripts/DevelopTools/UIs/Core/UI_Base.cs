using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UI_GoBack))]
public abstract class UI_Base : MonoBehaviour
{
    [Header("UI_Base")]
    public GameObject firstToOpenMenu;
    [System.NonSerialized] public int idfinder;
    [System.NonSerialized] public bool isActive;

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
    void Start() => OnStart();
    void EndCloseAnimation() => OnEndCloseAnimation();
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
    public void ConfigurateFirst(GameObject go) => firstToOpenMenu = go;

    public virtual void Open()
    {
        anim.Open();
        parent.SetActive(true);
        Refresh();
        isActive = true;
        Main.instance.GetMyEventSystem().Set_First(firstToOpenMenu.gameObject);
    }
    public virtual void Close()
    {
        anim.Close();
        /*isActive = false; */
        Main.instance.GetMyEventSystem().DeselectGameObject();
    }
}
