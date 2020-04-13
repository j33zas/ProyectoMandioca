using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tools.Extensions;

public class SkillManager_Activas : MonoBehaviour
{
    //[Header("Agreguese las skills por acá en editor")]
    //[SerializeField] UI_SkillHandler frontend;
    //[SerializeField] List<SkillBase> allskills;
    //Dictionary<SkillType, SkillBase> currents = new Dictionary<SkillType, SkillBase>();

    public UI_SkillHandler_Activas frontend;

    [Header("All skills data base")]
    [SerializeField] List<SkillActivas> allskillsDatabase;
    Dictionary<SkillInfo, SkillActivas> fastreference;
    Dictionary<SkillInfo, Item> fastreference_item = new Dictionary<SkillInfo, Item>();

    public SkillActivas[] myActiveSkills = new SkillActivas[4];

    public SkillActivas vacio;

    public void EV_Up_DPad() => Press(0);
    public void EV_Left_DPad() => Press(1);
    public void EV_Down_DPad() => Press(2);
    public void EV_Right_DPad() => Press(3);

    public void Press(int index) 
    {
        var ui = myActiveSkills[index].GetUI();

        var event_data = new UnityEngine.EventSystems.BaseEventData(Main.instance.GetMyEventSystem().GetMyEventSystem());
        ui.OnSubmit(event_data);
    }

    public void Initialize()
    {
        myActiveSkills = new SkillActivas[4];
        for (int i = 0; i < myActiveSkills.Length; i++) myActiveSkills[i] = vacio;
        
        allskillsDatabase = GetComponentsInChildren<SkillActivas>().ToList();
        FillDiccionary();


        frontend.Refresh(myActiveSkills, OnUISelected);

        for (int i = 0; i < myActiveSkills.Length; i++) myActiveSkills[i].BeginSkill();
    }

    public void OnUISelected(int selected)
    {
        myActiveSkills[selected].Execute();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.G))
    //    {
    //        ReplaceFor(vacio.skillinfo, 0);
    //        ReplaceFor(vacio2.skillinfo, 1);
    //        frontend.Reconfigurate(myActiveSkills);
    //    }
    //}


    public SkillInfo Look(int index) => allskillsDatabase[index].skillinfo;
    int indextest;
    public void ReplaceFor(SkillInfo _skillinfo, int index)
    {

        myActiveSkills[indextest].EndSkill();
        myActiveSkills[indextest] = fastreference[_skillinfo];



        frontend.Reconfigurate(myActiveSkills);
        myActiveSkills[indextest].BeginSkill();

        indextest = indextest.NextIndex(myActiveSkills.Length);

        //spawnear el viejo
    }
    public void ReplaceFor(SkillInfo _skillinfo, int index, Item item)
    {

        foreach (var i in myActiveSkills)
        {
            if (_skillinfo == i.skillinfo)
            {
                var _item = fastreference_item[_skillinfo];
                Main.instance.SpawnItem(_item, Main.instance.GetChar().transform.position + Main.instance.GetChar().GetCharMove().GetRotatorDirection());

                return;
            }
        }


        myActiveSkills[indextest].EndSkill();

        if (fastreference_item.ContainsKey(myActiveSkills[indextest].skillinfo))
        {
            var _item = fastreference_item[myActiveSkills[indextest].skillinfo];
            Main.instance.SpawnItem(_item,Main.instance.GetChar().transform.position + Main.instance.GetChar().GetCharMove().GetRotatorDirection());
            //fastreference_item.Remove(myActiveSkills[indextest].skillinfo);
        }

        myActiveSkills[indextest] = fastreference[_skillinfo];

        if (!fastreference_item.ContainsKey(_skillinfo))
        {
            fastreference_item.Add(_skillinfo, item);
        }

        frontend.Reconfigurate(myActiveSkills);
        myActiveSkills[indextest].BeginSkill();

        indextest = indextest.NextIndex(myActiveSkills.Length);

        //spawnear el viejo
    }
    void FillDiccionary()
    {
        fastreference = new Dictionary<SkillInfo, SkillActivas>();
        foreach (var s in allskillsDatabase)
            if (!fastreference.ContainsKey(s.skillinfo))
                fastreference.Add(s.skillinfo, s);
    }

    //void DropSkill(SkillInfo skill) { }
    //void Refresh() { }
    //void OnUISelected(int i)
    //{
    //    //Debug.Log("Recibi:" + i);
    //    //var select = allskills[i];
    //    //var old = currents[select.skillinfo.skilltype];
    //    //old.EndSkill();
    //    //select.BeginSkill();
    //    //currents[select.skillinfo.skilltype] = select;
    //    //frontend.SetInfoSelected(select.skillinfo);
    //}
    //void Start()
    //{
    //    //// ahora esto es un start.
    //    //// pero tiene que venir de un OnSceneLoaded
    //    //allskills = GetComponentsInChildren<SkillBase>().ToList();
    //    //Build();
    //}
    //public void Build()
    //{
    //    //if (!dataisloaded) {
    //    //    foreach (var skill in allskills) {
    //    //        if (!currents.ContainsKey(skill.skillinfo.skilltype)) {
    //    //            currents.Add(skill.skillinfo.skilltype, skill);
    //    //        }
    //    //    }
    //    //}
    //    //frontend.Build(allskills,OnUISelected);
    //    //foreach (var s in currents.Values) s.BeginSkill();
    //}
}
