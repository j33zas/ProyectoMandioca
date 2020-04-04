using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("Agreguese las skills por acá en editor")]
    [SerializeField] List<SkillBase> allskills;
    Dictionary<SkillType, SkillBase> currents = new Dictionary<SkillType, SkillBase>();

    [SerializeField] UI_SkillHandler frontend;

    bool dataisloaded;
    public void LoadFromDisk(Dictionary<SkillType, SkillBase> dic) { currents = dic; dataisloaded = true; }

    void Start()
    {
        // ahora esto es un start.
        // pero tiene que venir de un OnSceneLoaded

        Build();
    }

    public void Build()
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

        foreach (var s in currents.Values)
        {
            s.BeginSkill();
        }

        frontend.Build(allskills);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
