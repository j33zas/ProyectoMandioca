using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("Agreguese las skills por acá en editor")]
    [SerializeField] UI_SkillHandler frontend;
    [SerializeField] List<SkillBase> allskills;
    Dictionary<SkillType, SkillBase> currents = new Dictionary<SkillType, SkillBase>();

    public bool isOpen = false;

    bool dataisloaded;
    public void LoadFromDisk(Dictionary<SkillType, SkillBase> dic) { currents = dic; dataisloaded = true; }

    void Start()
    {
        // ahora esto es un start.
        // pero tiene que venir de un OnSceneLoaded
        allskills = GetComponentsInChildren<SkillBase>().ToList();
        Build();
    }

    public void EVENT_OnbackButton()
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

    public void Build()
    {
        if (!dataisloaded) {
            foreach (var skill in allskills) {
                if (!currents.ContainsKey(skill.skillinfo.skilltype)) {
                    currents.Add(skill.skillinfo.skilltype, skill);
                }
            }
        }

        frontend.Build(allskills,OnUISelected);
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
}
