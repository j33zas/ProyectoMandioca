using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager_Pasivas : MonoBehaviour
{
    [Header("Data_base")]
    [SerializeField] List<SkillBase> allskills;
    public Dictionary<SkillInfo, SkillBase> database_basebyinfo = new Dictionary<SkillInfo, SkillBase>();

    [SerializeField] List<SkillLevelByBranch> alllevels;
    public Dictionary<SkillType, List<SkillLevelByBranch>> database_levelbytype = new Dictionary<SkillType, List<SkillLevelByBranch>>();
    
    List<SkillBase> current_list_of_skills = new List<SkillBase>();
    SkillType current_skill_type;
    SkillBase current_skills;

    Queue<SkillInfo[]> newSkillRequests = new Queue<SkillInfo[]>();

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////// ON LOAD BEGIN
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    public void Initialize()
    {
        allskills = GetComponentsInChildren<SkillBase>().ToList();
        RefillFastDiccionaries();
        Build_menu_for_testing();
    }
    void RefillFastDiccionaries()//no es necesario, pero lo tenemos para acceder mas rapido
    {
        foreach (var s in allskills)
        {
            if (!database_basebyinfo.ContainsKey(s.skillinfo))
            {
                database_basebyinfo.Add(s.skillinfo, s);
            }
        }
        foreach (var level in alllevels)
        {
            if (!database_levelbytype.ContainsKey(level.skilltype))
            {
                List<SkillLevelByBranch> list = new List<SkillLevelByBranch>();
                list.Add(level);
                database_levelbytype.Add(level.skilltype, list);
            }
            else
            {
                database_levelbytype[level.skilltype].Add(level);
            }
        }
    }
    public void SelectASkillType(SkillType _skilltype)
    {
        current_skill_type = _skilltype;
        Refresh();
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////// ON GAME
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///

    int requestindex = 0;
    public void CreateRequest_NewSkill()
    {
        Debug.Log("create a request");
        var req = database_levelbytype[current_skill_type][requestindex].Selection;
        newSkillRequests.Enqueue(req);
        Debug.Log("Req _" + newSkillRequests.Count);
        requestindex++;
    }
    public void EVENT_GetRequest()
    {
        Debug.Log("SPEND REQUEST");
        if (newSkillRequests.Count > 0)
        {
            Debug.Log("# SPEND REQUEST");
            var dequeuedRequest = newSkillRequests.Dequeue();
            Main.instance.gameUiController.CreateNewSkillSelectionPopUp(dequeuedRequest.ToList(), ReturnSkill);
        }
    }
    public bool I_Have_An_Active_Request() => newSkillRequests.Count() > 0;

    void ReturnSkill(SkillInfo info)
    {
        if (!current_list_of_skills.Contains(database_basebyinfo[info]))
        {
            current_list_of_skills.Add(database_basebyinfo[info]);
        }

        Refresh();
    }
    public void Refresh()
    {
        foreach (var s in current_list_of_skills)
        {
            s.BeginSkill();
        }
        Main.instance.gameUiController.UI_Send_NameSkillType(current_skill_type.ToString());
        Main.instance.gameUiController.RefreshPassiveSkills_UI(current_list_of_skills.Select(x => x.skillinfo).ToList());
    }
    

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////// FOR TESTING MENU
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    [Header("Testing Menu")]
    public bool testing;
    bool isOpen = false;
    Dictionary<SkillType, SkillBase> currents = new Dictionary<SkillType, SkillBase>();
    public List<SkillInfo> skillinfos;
    [SerializeField] UI_SkillHandler frontend;
    bool dataisloaded;
    public void LoadFromDisk(Dictionary<SkillType, SkillBase> dic) { currents = dic; dataisloaded = true; }
    public void Build_menu_for_testing()
    {
        if (!dataisloaded)
        {
            foreach (var skill in allskills)
            {
                if (!currents.ContainsKey(skill.skillinfo.skilltype))
                {
                    currents.Add(skill.skillinfo.skilltype, skill);
                }
            }
        }
        frontend.Build(allskills, OnUISelected);
        foreach (var s in currents.Values) s.BeginSkill();
    }
    void OnUISelected(int i)
    {
        var select = allskills[i];
        var old = currents[select.skillinfo.skilltype];

        old.EndSkill();
        select.BeginSkill();
        currents[select.skillinfo.skilltype] = select;

        frontend.SetInfoSelected(select.skillinfo);
    }
    public void EVENT_OnbackButton()
    {
        if (testing)
        {
            if (!isOpen)
            {
                isOpen = true;
                frontend.Open();
                //lo abrimos
            }
            else
            {
                isOpen = false;
                frontend.Close();
                //lo cerramos
            }
        }
    }
}

[System.Serializable]
public class SkillsPerLevel
{
    int level = 0;
    SkillInfo[] skills;
}

[System.Serializable]
public class NewSkillRequest
{
    SkillInfo[] skills;
}
