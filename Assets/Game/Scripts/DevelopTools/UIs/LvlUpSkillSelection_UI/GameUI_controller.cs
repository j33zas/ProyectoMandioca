using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using UnityEngine.UI;

public class GameUI_controller : MonoBehaviour
{
    public static GameUI_controller instance;

    [SerializeField] private GameObject skillSelection_template_pf;
    [SerializeField] private GameObject charStats_template_pf;
    [SerializeField] private WorldCanvasPopUp lvlUp_pf;
    
    private CharStats_UI _charStats_Ui;
    Dictionary<UI_templates, GameObject> UiTemplateRegistry = new Dictionary<UI_templates, GameObject>();

    [Header("--XX--Canvas containers--XX--")]
    [SerializeField] private RectTransform leftCanvas;
    [SerializeField] private RectTransform rightCanvas;

    [SerializeField] Canvas myCanvas; public Canvas MyCanvas { get => myCanvas;}

    public bool openUI { get; private set; }
   

    #region Config

    void Awake()
    {
        if (instance == null)
            instance = this;
        
        RegistrarUIPrefabs();
    }

    private void Start()
    {
        _charStats_Ui = Instantiate<GameObject>(UiTemplateRegistry[UI_templates.charStats], leftCanvas).GetComponent<CharStats_UI>();
    }

    private void RegistrarUIPrefabs()
    {
        UiTemplateRegistry.Add(UI_templates.skillSelection, skillSelection_template_pf);
        UiTemplateRegistry.Add(UI_templates.charStats, charStats_template_pf);
    }

    #endregion

    #region Public methods

    public void Set_Opened_UI()
    {
        openUI = true; Main.instance.Pause();
    }
    public void Set_Closed_UI() { openUI = false; Main.instance.Play(); }

    /// <summary>
    /// Creas el popUp para elegir skill.
    /// El callback recibe un skillinfo. Ese skillInfo es el seleccionado.
    /// </summary>
    /// <param name="uiTemplates"></param>
    /// <returns></returns>
    public LvlUpSkillSelection_UI CreateNewSkillSelectionPopUp(List<SkillInfo> skillsParaElegir, Action<SkillInfo> callback)
    {
        LvlUpSkillSelection_UI newPopUp = Instantiate(UiTemplateRegistry[UI_templates.skillSelection], leftCanvas).GetComponent<LvlUpSkillSelection_UI>();
        newPopUp.Configure(skillsParaElegir, callback);
        return newPopUp;
    }

    public void UI_Send_NameSkillType(string s)
    {
        //el nombre de el tipo de skill
    }

    public void UI_SendLevelUpNotification()
    {
        
        CanvasPopUpInWorld_Manager.instance.MakePopUpAnimated(Main.instance.GetChar().transform, lvlUp_pf);
    }
    public void UI_SendActivePlusNotification(bool val)
    {
        Debug.Log(val);
        //aca activo o desactivo la lucecita o el algo que indique que puedo elegir una skill
        if(val) _charStats_Ui.ToggleLvlUpSign();
            
    }
    public void UI_RefreshExpBar(int currentExp, int maxExp, int currentLevel)
    {
        //aca lo mando a una barrita que refresque todo esto
        //y me muestre el nivel y la experienca acumulada
        _charStats_Ui.UpdateXP_UI(currentExp,maxExp, currentLevel);
        
    }

    public void RefreshPassiveSkills_UI(List<SkillInfo> skillsNuevas)
    {
        _charStats_Ui.UpdatePasiveSkills(skillsNuevas);   
    }


    #endregion


    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {

            Debug.Log("Abro el menu");
            if (Main.instance.Ui_Is_Open())
                Set_Closed_UI();
            else
                Set_Opened_UI();

        }
    }
}
