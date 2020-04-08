using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI_controller : MonoBehaviour
{
    public static GameUI_controller instance;

    [SerializeField] private GameObject skillSelection_template_pf;
    
    Dictionary<UI_templates, GameObject> UiTemplateRegistry = new Dictionary<UI_templates, GameObject>();

    [SerializeField] private RectTransform leftCanvas;
    [SerializeField] private RectTransform rightCanvas;

    public List<SkillInfo> skillsTEST;
    void Awake()
    {
        if (instance == null)
            instance = this;
        
        RegistrarUIPrefabs();
    }

    private void RegistrarUIPrefabs()
    {
        UiTemplateRegistry.Add(UI_templates.skillSelection, skillSelection_template_pf);
    }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CreateNewSkillSelectionPopUp(skillsTEST, info =>Debug.Log(info.skill_name) );
        }
    }
}
