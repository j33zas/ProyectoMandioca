using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tools.Extensions;

public class SkillManager_Activas : MonoBehaviour
{
    public UI_SkillHandler_Activas frontend;

    [Header("All skills data base")]
    [SerializeField] List<SkillActivas> my_editor_data_base;
    Dictionary<SkillInfo, SkillActivas> fastreference_actives;
    Dictionary<SkillInfo, Item> fastreference_item = new Dictionary<SkillInfo, Item>();

    public SkillActivas[] myActiveSkills = new SkillActivas[4];

    public SkillActivas vacio;

    public Item[] items_to_spawn;

    bool percenslot = true;

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_INITIALIZE, Initialize);
    }

    void Initialize()
    {
        //obtengo la data base de mis childrens
        my_editor_data_base = GetComponentsInChildren<SkillActivas>().ToList();

        //las relleno con el item vacio
        myActiveSkills = new SkillActivas[4];
        for (int i = 0; i < myActiveSkills.Length; i++) myActiveSkills[i] = vacio;

        //relleno el diccionario de acceso rapido
        fastreference_actives = new Dictionary<SkillInfo, SkillActivas>();
        foreach (var s in my_editor_data_base)
            if (!fastreference_actives.ContainsKey(s.skillinfo))
                fastreference_actives.Add(s.skillinfo, s);

        //refresco la ui con mis skills vacios
        frontend.Refresh(myActiveSkills, OnUISelected);

        //¿esto? no recuerdo porque esta acá...
        for (int i = 0; i < myActiveSkills.Length; i++) myActiveSkills[i].BeginSkill();

        //otras cosas
        //(spawn de enemigos) esto lo hago aca porque quiero tener el control de spawn acá... 
        //para tener un roullete que no me tire siempre los mismos items que ya tengo equipado
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ENEMY_DEAD, EnemyDeath);

        //refresco el sistema de slots x vida
        ReceiveLife((int)Main.instance.GetChar().Life.GetLife(), 100);

        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Sistema de Slots por porcentaje de vida", true, UseMode_Slots);
    }

    string UseMode_Slots(bool b)
    {
        percenslot = b;
        ReceiveLife((int)Main.instance.GetChar().Life.GetLife(), 100);
        return "PercentMode: " + (b ? "activated" : "deactivated") ;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// INPUT
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void EV_Up_DPad() => Press(0);
    public void EV_Left_DPad() => Press(1);
    public void EV_Down_DPad() => Press(2);
    public void EV_Right_DPad() => Press(3);

    bool normaluse = true;
    public void Press(int index)
    {
        if (normaluse)
        {
            var ui = myActiveSkills[index].GetUI();
            var event_data = new UnityEngine.EventSystems.BaseEventData(Main.instance.GetMyEventSystem().GetMyEventSystem());
            ui.OnSubmit(event_data);
        }
        else
        {
            //aca remplazo
        }
    }


    const int MAX_PERCENT = 100;
    readonly int[] percentedvalues = new int[] { 0, 25, 50, 75 };
    public bool[] slots = new bool[4];
    public void ReceiveLife(int _life, int max)
    {
        Debug.Log("Life: " + _life + " Max: " + max);
        var aux_value = MAX_PERCENT - _life;
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

    public SkillInfo Look(int index) => my_editor_data_base[index].skillinfo;
    int current_index;
    public void ReplaceFor(SkillInfo _skillinfo, int index)
    {
        myActiveSkills[current_index].EndSkill();
        myActiveSkills[current_index] = fastreference_actives[_skillinfo];

        frontend.Reconfigurate(myActiveSkills);
        myActiveSkills[current_index].BeginSkill();

        current_index = current_index.NextIndex(myActiveSkills.Length);

        //spawnear el viejo
    }
    bool oneshot;
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
        if (!oneshot) { oneshot = true; cleanindex = 0; }

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
        myActiveSkills[cleanindex] = fastreference_actives[_skillinfo];

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

    
}
