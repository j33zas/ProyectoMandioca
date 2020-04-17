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

    public Item[] items_to_spawn;

    bool percenslot;

    public void Initialize()
    {
        myActiveSkills = new SkillActivas[4];
        for (int i = 0; i < myActiveSkills.Length; i++) myActiveSkills[i] = vacio;

        allskillsDatabase = GetComponentsInChildren<SkillActivas>().ToList();
        FillDiccionary();


        frontend.Refresh(myActiveSkills, OnUISelected);

        for (int i = 0; i < myActiveSkills.Length; i++) myActiveSkills[i].BeginSkill();
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ENEMY_DEAD, EnemyDeath);

        ReceiveLife((int)Main.instance.GetChar().Life.GetLife(), 100);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// INPUT
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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


    const int MAX = 100;
    readonly int[] percentedvalues = new int[] { 0, 25, 50, 75 };
    public bool[] slots = new bool[4];
    public void ReceiveLife(int _life, int max)
    {
        //ahora sabemos que 100 es el maximo, agregarle que lo calcule con el maximo sacando porcentaje

        var aux_value = MAX - _life;
        if (aux_value == 0) aux_value = 1;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = aux_value > percentedvalues[i];
        }

        if (!percenslot)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = true;
            }
        }

        frontend.RefreshButtons(slots);
    }

   

    void EnemyDeath(params object[] param)
    {
        Main.instance.SpawnItem(items_to_spawn[Random.Range(0, items_to_spawn.Length)], (Vector3)param[0]);
    }

    public void OnUISelected(int selected)
    {
        myActiveSkills[selected].Execute();
    }

    public SkillInfo Look(int index) => allskillsDatabase[index].skillinfo;
    int current_index;
    public void ReplaceFor(SkillInfo _skillinfo, int index)
    {

        myActiveSkills[current_index].EndSkill();
        myActiveSkills[current_index] = fastreference[_skillinfo];



        frontend.Reconfigurate(myActiveSkills);
        myActiveSkills[current_index].BeginSkill();

        current_index = current_index.NextIndex(myActiveSkills.Length);

        //spawnear el viejo
    }
    public bool ReplaceFor(SkillInfo _skillinfo, int index, Item item)
    {
        //si ya la tengo repetida ni la agarro
        foreach (var i in myActiveSkills) if (_skillinfo == i.skillinfo) return false; else continue;
        //ahora si no la tengo repetida

        if (!fastreference_item.ContainsKey(_skillinfo))
        {
            fastreference_item.Add(_skillinfo, item);
        }


        //esto es una negrada... pero hasta que no se confirme si se va a hacer lo de magno
        //lo que hace es que si usa el sistema de magno no se buguee al entrar
        if (slots[0] == false)
        {
            for (int i = 0; i < slots.Length; i++) slots[i] = true;
        }

        int cleanindex = FindNextCleanIndex(current_index);

        //endskill del anterior
        myActiveSkills[cleanindex].EndSkill();

        //si mi diccionario biblioteca de items contiene el info del anterior
        //spawneo el item
        //si entro aca es porque quiere decir que alguna vez un item que puede ser dropeado entró aca, por lo tanto no va a estar vacio
        if (fastreference_item.ContainsKey(myActiveSkills[cleanindex].skillinfo))
        {
            //obtengo el item del anterior
            var _item = fastreference_item[myActiveSkills[cleanindex].skillinfo];
            //spawneo el item anterior
            Main.instance.SpawnItem(_item, Main.instance.GetChar().transform.position + Main.instance.GetChar().GetCharMove().GetRotatorDirection());
        }

        //asigno a este index el nuevo skill
        myActiveSkills[cleanindex] = fastreference[_skillinfo];

        frontend.Reconfigurate(myActiveSkills);
        myActiveSkills[cleanindex].BeginSkill();

        current_index = cleanindex;

        return true;
        //spawnear el viejo
    }
    int FindNextCleanIndex(int current)
    {
        int next_clean_index = current;
        next_clean_index = next_clean_index.NextIndex(myActiveSkills.Length);
        while (!slots[next_clean_index]) next_clean_index = next_clean_index.NextIndex(myActiveSkills.Length);
        return next_clean_index;
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



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////// REQUEST TIME BARS
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dictionary<SkillInfo, int> fastindex = new Dictionary<SkillInfo, int>();
    //Queue<SkillInfo> skill_use_refresh = new Queue<SkillInfo>();
    //public void StartRequestBar(SkillInfo _idInfo)
    //{
    //}
    //public void StopRequestBar(SkillInfo _idInfo)
    //{
    //    skill_use_refresh
    //}
    //public void RefreshUseTimeBar(SkillInfo _idInfo, float current_timer, float max_Value)
    //{
    //    //aca hago refresh de ui... asi tengo control de la ejecucion
    //}
    //public class RequestTimeBar
    //{
    //    SkillInfo info;
    //    float current_timer; public float Current { get => current_timer; }
    //    float max_value; public float Max { get => max_value; }
    //    public RequestTimeBar(SkillInfo _info) { info = _info; }
    //    public void RefreshData(float current, float max) { current_timer = current; max_value = max; }
    //}
}
